using System;
using Verse;
using RimWorld;
using HarmonyLib;

namespace Adrenaline
{

    public static class Patch_TraitSet
    {

        [HarmonyPatch(typeof(TraitSet))]
        [HarmonyPatch(nameof(TraitSet.GainTrait))]
        public static class Patch_GainTrait
        {

            public static bool Prefix(TraitSet __instance, Pawn ___pawn, Trait trait)
            {
                return trait.def != A_TraitDefOf.AdrenalineJunkie && !AffectsAdrenalineProduction(trait) || ___pawn.CanGetAdrenaline();
            }

        }

        private static bool AffectsAdrenalineProduction(Trait trait)
        {
            // If the trait has degrees, check the current degree
            if (!trait.def.degreeDatas.NullOrEmpty())
            {
                var traitData = trait.CurrentData;

                // Check all statOffsets
                if (!traitData.statOffsets.NullOrEmpty())
                    foreach (var statOffset in traitData.statOffsets)
                        if (statOffset.stat == A_StatDefOf.AdrenalineProduction)
                            return true;

                // Check all statFactors
                if (!traitData.statFactors.NullOrEmpty())
                    foreach (var statFactors in traitData.statFactors)
                        if (statFactors.stat == A_StatDefOf.AdrenalineProduction)
                            return true;
            }
            return false;
        }

    }

}
