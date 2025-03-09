using HarmonyLib;
using RimWorld;
using Verse;

namespace Adrenaline;

[HarmonyPatch(typeof(TraitSet), nameof(TraitSet.GainTrait))]
public static class TraitSet_GainTrait
{
    private static bool AffectsAdrenalineProduction(Trait trait)
    {
        // If the trait has degrees, check the current degree
        if (trait.def.degreeDatas.NullOrEmpty())
        {
            return false;
        }

        var traitData = trait.CurrentData;

        // Check all statOffsets
        if (!traitData.statOffsets.NullOrEmpty())
        {
            foreach (var statOffset in traitData.statOffsets)
            {
                if (statOffset.stat == A_StatDefOf.AdrenalineProduction)
                {
                    return true;
                }
            }
        }

        // Check all statFactors
        if (traitData.statFactors.NullOrEmpty())
        {
            return false;
        }

        foreach (var statFactors in traitData.statFactors)
        {
            if (statFactors.stat == A_StatDefOf.AdrenalineProduction)
            {
                return true;
            }
        }

        return false;
    }

    public static bool Prefix(Pawn ___pawn, Trait trait)
    {
        return trait.def != A_TraitDefOf.AdrenalineJunkie && !AffectsAdrenalineProduction(trait) ||
               ___pawn.CanGetAdrenaline();
    }
}