using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Adrenaline
{
    public static class Patch_Pawn
    {
        [HarmonyPatch(typeof(Pawn), nameof(Pawn.GetGizmos))]
        public static class Patch_GetGizmos
        {
            public static void Postfix(Pawn __instance, ref IEnumerable<Gizmo> __result)
            {
                var localResult = __result.ToList();
                // If the pawn is downed, is a colonist and can take adrenaline, add a 'take adrenaline' gizmo
                if (!__instance.Downed || !__instance.IsColonistPlayerControlled || !__instance.CanGetAdrenaline())
                {
                    return;
                }

                var extraRaceProps = __instance.def.GetModExtension<ExtendedRaceProperties>() ??
                                     ExtendedRaceProperties.defaultValues;
                foreach (var tDef in extraRaceProps.RelevantConsumablesDowned)
                {
                    var thingDefExtension =
                        tDef.GetModExtension<ThingDefExtension>() ?? ThingDefExtension.defaultValues;

                    var anyNearbyAdrenaline =
                        AdrenalineUtility.AnyNearbyAdrenaline(__instance, tDef, out var adrenalineThings);

                    var adrenalineGizmo = new Command_Action
                    {
                        defaultLabel = thingDefExtension.downedIngestGizmoLabel,
                        defaultDesc = thingDefExtension.downedIngestGizmoDescription
                    };

                    if (AdrenalineUtility.adrenalineGizmoIcons.TryGetValue(tDef, out var icon))
                    {
                        adrenalineGizmo.icon = icon;
                    }

                    // No adrenaline nearby
                    if (!anyNearbyAdrenaline)
                    {
                        adrenalineGizmo.Disable(thingDefExtension.downedIngestGizmoNoneNearby);
                        localResult.Add(adrenalineGizmo);
                        continue;
                    }

                    // Can't do manipulation
                    if (!__instance.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
                    {
                        adrenalineGizmo.Disable(
                            "Adrenaline.Command_TakeAdrenaline_FailNoManipulation"
                                .Translate(__instance.LabelShort));
                        localResult.Add(adrenalineGizmo);
                        continue;
                    }

                    adrenalineGizmo.action = () => adrenalineThings[0].Ingested(__instance, 0);
                    localResult.Add(adrenalineGizmo);
                }
            }
        }
    }
}