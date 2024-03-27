using HarmonyLib;
using Verse;

namespace Adrenaline;

public static class Patch_VerbProperties
{
    [HarmonyPatch(typeof(VerbProperties))]
    [HarmonyPatch(nameof(VerbProperties.GetDamageFactorFor))]
    [HarmonyPatch([typeof(Tool), typeof(Pawn), typeof(HediffComp_VerbGiver)])]
    public static class Patch_GetDamageFactorFor
    {
        public static void Postfix(Pawn attacker, ref float __result)
        {
            // If an attacker exists, go through each hediff and multiply damage by the hediff's melee damage factor
            if (attacker == null)
            {
                return;
            }

            foreach (var hediff in attacker.health.hediffSet.hediffs)
            {
                var hediffDefExtension = hediff.def.GetModExtension<HediffDefExtension>() ??
                                         HediffDefExtension.defaultValues;
                __result *= hediffDefExtension.GetExtraHediffStagePropertiesAt(hediff.CurStageIndex)
                    .meleeDamageFactor;
            }
        }
    }
}