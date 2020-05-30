using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;

namespace Adrenaline
{

    public static class Patch_Pawn
    {

        [HarmonyPatch(typeof(Pawn), nameof(Pawn.GetGizmos))]
        public static class Patch_GetGizmos
        {

            public static void Postfix(Pawn __instance, ref IEnumerable<Gizmo> __result)
            {
                var localResult = __result.ToList<Gizmo>();
                // If the pawn is downed, is a colonist and can take adrenaline, add a 'take adrenaline' gizmo
                if (__instance.Downed && __instance.IsColonistPlayerControlled && __instance.CanGetAdrenaline())
                {
                    var extraRaceProps = __instance.def.GetModExtension<ExtendedRaceProperties>() ?? ExtendedRaceProperties.defaultValues;
                    foreach (var tDef in extraRaceProps.RelevantConsumablesDowned)
                    {
                        var thingDefExtension = tDef.GetModExtension<ThingDefExtension>() ?? ThingDefExtension.defaultValues;

                        bool anyNearbyAdrenaline = AdrenalineUtility.AnyNearbyAdrenaline(__instance, tDef, out List<Thing> adrenalineThings);

                        var adrenalineGizmo = new Command_Action()
                        {
                            defaultLabel = thingDefExtension.downedIngestGizmoLabel,
                            defaultDesc = thingDefExtension.downedIngestGizmoDescription
                        };

                        if (AdrenalineUtility.adrenalineGizmoIcons.TryGetValue(tDef, out Texture2D icon))
                            adrenalineGizmo.icon = icon;

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
                            adrenalineGizmo.Disable("Adrenaline.Command_TakeAdrenaline_FailNoManipulation".Translate(__instance.LabelShort));
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

}
