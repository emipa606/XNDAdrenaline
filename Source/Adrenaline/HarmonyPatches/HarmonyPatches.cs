using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace Adrenaline;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    static HarmonyPatches()
    {
        Adrenaline.HarmonyInstance.PatchAll();

        try
        {
            // PawnInventoryGenerator.GiveCombatEnhancingDrugs source predicate
            Patch_PawnInventoryGenerator.ManualPatch_GiveCombatEnhancingDrugs_source_predicate.predicateType =
                typeof(PawnInventoryGenerator).GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance)
                    .First(t => t.Name.Contains("GiveCombatEnhancingDrugs"));

            Adrenaline.HarmonyInstance.Patch(
                Patch_PawnInventoryGenerator.ManualPatch_GiveCombatEnhancingDrugs_source_predicate.predicateType
                    .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                    .First(m => m.ReturnType == typeof(bool)),
                transpiler: new HarmonyMethod(
                    typeof(Patch_PawnInventoryGenerator.ManualPatch_GiveCombatEnhancingDrugs_source_predicate),
                    "Transpiler"));
        }
        catch (Exception exception)
        {
            if (Prefs.DevMode)
            {
                Log.Message(exception.ToString());
            }
        }
    }
}