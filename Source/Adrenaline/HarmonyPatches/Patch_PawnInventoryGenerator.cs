using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

namespace Adrenaline
{
    public static class Patch_PawnInventoryGenerator
    {
        public static class ManualPatch_GiveCombatEnhancingDrugs_source_predicate
        {
            public static Type predicateType;

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                var instructionList = instructions.ToList();

                var pawnInfo = AccessTools.Field(predicateType, "pawn");
                var modifyResultInfo = AccessTools.Method(typeof(ManualPatch_GiveCombatEnhancingDrugs_source_predicate),
                    nameof(ModifyResult));

                foreach (var instruction in instructionList)
                {
                    // Wherever the predicate returns, add a call to our method which can modify the return value
                    if (instruction.opcode == OpCodes.Ret)
                    {
                        yield return new CodeInstruction(OpCodes.Ldarg_1); // x
                        yield return new CodeInstruction(OpCodes.Ldarg_0); // this
                        yield return new CodeInstruction(OpCodes.Ldfld, pawnInfo); // this.pawn
                        yield return
                            new CodeInstruction(OpCodes.Call, modifyResultInfo); // ModifyResult(result, x, this.pawn)
                    }

                    yield return instruction;
                }
            }

            private static bool ModifyResult(bool result, ThingDef x, Pawn p)
            {
                // Prevent pawns from generating with the wrong adrenaline-giving drugs if applicable
                if (!result || !x.ingestible.outcomeDoers.Any(o => o is IngestionOutcomeDoer_Adrenaline))
                {
                    return result;
                }

                var extraRaceProps = p.def.GetModExtension<ExtendedRaceProperties>() ??
                                     ExtendedRaceProperties.defaultValues;
                return extraRaceProps.RelevantConsumables.Contains(x);
            }
        }
    }
}