using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace Adrenaline;

public static class AdrenalineUtility
{
    private const float BasePerceivedThreatDistance = 50f;

    private static readonly SimpleCurve pointsPerColonistByWealthCurve =
    [
        new CurvePoint(0f, 15f),
        new CurvePoint(10000f, 15f),
        new CurvePoint(400000f, 140f),
        new CurvePoint(1000000f, 200f)
    ];

    public static readonly Dictionary<ThingDef, Texture2D> AdrenalineGizmoIcons = new();

    public static IEnumerable<Thing> GetPerceivedThreatsFor(Pawn pawn)
    {
        if (pawn?.Map == null)
        {
            yield break;
        }

        var potentialThings = pawn.Map.GetComponent<MapComponent_AdrenalineTracker>()?.allPotentialHostileThings;
        if (potentialThings == null)
        {
            yield break;
        }

        foreach (var threat in potentialThings)
        {
            if (threat.isPerceivedThreatBy(pawn))
            {
                yield return threat;
            }
        }
    }

    /// <summary>
    /// Fast early-exit threat check. Returns true on the very first threat found without evaluating the rest of the map.
    /// </summary>
    public static bool HasPerceivedThreat(Pawn pawn)
    {
        if (pawn?.Map == null)
        {
            return false;
        }

        var potentialThings = pawn.Map.GetComponent<MapComponent_AdrenalineTracker>()?.allPotentialHostileThings;
        if (potentialThings == null)
        {
            return false;
        }

        foreach (var threat in potentialThings)
        {
            if (threat.isPerceivedThreatBy(pawn))
            {
                return true;
            }
        }

        return false;
    }

    private static bool isPerceivedThreatBy(this Thing t, Pawn pawn)
    {
        // 1. Basic map and validity checks
        if (t == null || pawn == null || !t.Spawned || t.Map == null || t.Map != pawn.Map)
        {
            return false;
        }

        // 2. Check pawn sight capacity
        float sightLevel = pawn.health.capacities.GetLevel(PawnCapacityDefOf.Sight);
        if (sightLevel <= 0f)
        {
            return false;
        }

        // 3. Fast distance check
        float maxDist = BasePerceivedThreatDistance * sightLevel;
        if (pawn.Position.DistanceTo(t.Position) > maxDist)
        {
            return false;
        }

        // 4. Fog check
        if (t.Position.Fogged(t.Map))
        {
            return false;
        }

        // 5. Cheap hostility & status checks BEFORE executing expensive line-of-sight raycasts
        bool isThreat = false;

        if (t is Pawn p)
        {
            var comp = p.GetComp<CompCanBeDormant>();
            if (comp is { Awake: false })
            {
                return false;
            }

            if (!p.Downed && (p.HostileTo(pawn) || pawn.inCombatWith(p)))
            {
                isThreat = true;
            }
        }
        else if (!pawn.RaceProps.Animal && t is Building_Turret turret)
        {
            var powerComp = turret.GetComp<CompPowerTrader>();
            if (powerComp is { PowerOn: false })
            {
                return false;
            }

            var mannableComp = turret.GetComp<CompMannable>();
            if (mannableComp is { MannedNow: false })
            {
                return false;
            }

            if (turret.CurrentEffectiveVerb != null && turret.CurrentEffectiveVerb.Available() && turret.HostileTo(pawn))
            {
                isThreat = true;
            }
        }

        if (!isThreat)
        {
            return false;
        }

        // 6. Raycast line-of-sight ONLY if the target passed all hostility, power, and range filters
        return pawn.CanSee(t);
    }

    public static bool CanGetAdrenaline(this Pawn p)
    {
        return p.def.CanGetAdrenaline();
    }

    public static bool CanGetAdrenaline(this ThingDef tDef)
    {
        var extraRaceProps = tDef.GetModExtension<ExtendedRaceProperties>() ?? ExtendedRaceProperties.defaultValues;
        return tDef.race != null && extraRaceProps.HasAdrenaline &&
               (tDef.race.hediffGiverSets?.Any(h =>
                   h.hediffGivers.Any(g => g.GetType() == typeof(HediffGiver_Adrenaline))) ?? false);
    }

    public static float TotalPainFactor(this Pawn pawn)
    {
        float factor = 1;
        foreach (var hediff in pawn.health.hediffSet.hediffs)
        {
            factor *= hediff.PainFactor;
        }

        return factor;
    }

    public static bool AnyNearbyAdrenaline(Thing t, List<ThingDef> aDefs, out List<Thing> adrenalineThings)
    {
        adrenalineThings = [];

        if (t.Map == null)
        {
            return false;
        }

        // Go through each cell adjacent to t, then check that cell for if it has any things that match anything in aDefs
        foreach (var c in t.CellsAdjacent8WayAndInside())
        {
            if (!c.InBounds(t.Map))
            {
                continue;
            }

            foreach (var aDef in aDefs)
            {
                var thing = c.GetFirstThing(t.Map, aDef);
                if (thing is { IngestibleNow: true })
                {
                    adrenalineThings.Add(thing);
                }
            }
        }

        return adrenalineThings.Any();
    }

    public static bool AnyNearbyAdrenaline(Thing t, ThingDef aDef, out List<Thing> adrenalineThings)
    {
        return AnyNearbyAdrenaline(t, [aDef], out adrenalineThings);
    }

    extension(Pawn pawn)
    {
        private bool inCombatWith(Pawn p)
        {
            // pawn is actively targeting p
            if (pawn.isAttacking(p))
            {
                return true;
            }

            // p is actively targeting pawn and has made attacks
            var battle = pawn.records.BattleActive;
            return p.isAttacking(pawn) && battle != null && battle.Concerns(p);
        }

        private bool isAttacking(Pawn p)
        {
            return pawn.IsFighting() && pawn.CurJob.AnyTargetIs(p);
        }
    }

    extension(Thing t)
    {
        public bool IsPotentialPerceivableThreat()
        {
            return t is Building_Turret;
        }

        public float PerceivedThreatSignificanceFor(Pawn pawn)
        {
            var thingWithComps = t as ThingWithComps;
            var p = t as Pawn;
            float threatSignificance = 0;

            // If the adrenaline gainee is an animal, only factor in the other thing's body size relative to the animal's body size
            if (pawn.RaceProps.Animal)
            {
                if (p != null)
                {
                    threatSignificance += p.BodySize * p.health.summaryHealth.SummaryHealthPercent /
                                          (pawn.BodySize * pawn.health.summaryHealth.SummaryHealthPercent);
                }
            }

            // Otherwise factor in 'effective combat power'
            else
            {
                threatSignificance += t.EffectiveCombatPower() / pawn.EffectiveCombatPower();

                // If threat is either manning a thing or being manned, halve the significance to reduce overlap
                if (p != null && p.MannedThing() != null || thingWithComps != null &&
                    thingWithComps.GetComp<CompMannable>() is { MannedNow: true })
                {
                    threatSignificance /= 2;
                }
            }

            // If pawn is attacking p but p isn't attacking pawn, reduce significance by 5/6
            if (p != null && pawn.isAttacking(p) && !p.isAttacking(pawn))
            {
                threatSignificance /= 6;
            }

            return threatSignificance;
        }

        private float EffectiveCombatPower()
        {
            switch (t)
            {
                // Pawn
                case Pawn p:
                {
                    float combatPower;

                    // If the pawn is a colonist, calculate combat power using the pawn's map wealth directly
                    if (p.IsColonist)
                    {
                        float wealth = p.Map?.PlayerWealthForStoryteller ?? 0f;
                        combatPower = Mathf.Max(
                            pointsPerColonistByWealthCurve.Evaluate(wealth),
                            p.kindDef.combatPower);
                    }
                    else
                    {
                        combatPower = p.kindDef.combatPower;
                    }

                    return combatPower * p.health.summaryHealth.SummaryHealthPercent *
                           p.ageTracker.CurLifeStage.bodySizeFactor;
                }
                // Turret
                case Building_Turret turret:
                    // Return 1/6th of its base market value
                    return turret.def.GetStatValueAbstract(StatDefOf.MarketValue) / 6;
                default:
                    return 0f;
            }
        }
    }
}