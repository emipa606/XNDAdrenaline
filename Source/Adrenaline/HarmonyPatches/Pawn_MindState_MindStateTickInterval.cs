﻿using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

namespace Adrenaline;

[HarmonyPatch(typeof(Pawn_MindState), nameof(Pawn_MindState.MindStateTickInterval))]
public static class Pawn_MindState_MindStateTickInterval
{
    public static void Postfix(Pawn_MindState __instance)
    {
        if (!AdrenalineSettings.NpcUse)
        {
            return;
        }

        var pawn = __instance.pawn;
        // Try to inject nearby adrenaline items if the pawn is downed, not a player pawn, is at least a tooluser, is capable of manipulation and moving, and can gain an adrelaine hediff
        if (!pawn.IsHashIntervalTick(60) || !pawn.Downed || pawn.Faction == Faction.OfPlayer ||
            pawn.RaceProps.intelligence < Intelligence.ToolUser ||
            !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation) ||
            !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Moving))
        {
            return;
        }

        var extraRaceProps = pawn.def.GetModExtension<ExtendedRaceProperties>() ??
                             ExtendedRaceProperties.defaultValues;
        if (extraRaceProps.adrenalineRushHediff == null)
        {
            return;
        }

        var adrenalineHediff =
            pawn.health.hediffSet.GetFirstHediffOfDef(extraRaceProps.adrenalineRushHediff);
        if ((adrenalineHediff == null ||
             adrenalineHediff.CurStageIndex < adrenalineHediff.def.stages.Count - 1) &&
            AdrenalineUtility.AnyNearbyAdrenaline(pawn, extraRaceProps.RelevantConsumablesDowned,
                out var adrenalineThings))
        {
            adrenalineThings[0].Ingested(pawn, 0);
        }
    }
}