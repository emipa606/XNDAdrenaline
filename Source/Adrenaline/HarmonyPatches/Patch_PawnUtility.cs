﻿using HarmonyLib;
using RimWorld;
using Verse;

namespace Adrenaline;

public static class Patch_PawnUtility
{
    [HarmonyPatch(typeof(PawnUtility))]
    [HarmonyPatch(nameof(PawnUtility.IsFighting))]
    public static class Patch_IsFighting
    {
        public static void Postfix(Pawn pawn, ref bool __result)
        {
            // If the method returned false but the pawn is doing a humanlike hunting job, make the method return true
            if (!__result && pawn.CurJobDef == JobDefOf.Hunt)
            {
                __result = true;
            }
        }
    }
}