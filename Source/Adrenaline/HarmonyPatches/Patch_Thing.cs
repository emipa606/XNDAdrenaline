﻿using HarmonyLib;
using Verse;

namespace Adrenaline;

public static class Patch_Thing
{
    [HarmonyPatch(typeof(Thing))]
    [HarmonyPatch(nameof(Thing.SpawnSetup))]
    public static class Patch_SpawnSetup
    {
        public static void Postfix(Thing __instance)
        {
            if (__instance.Map != null && __instance.IsPotentialPerceivableThreat())
            {
                __instance.Map.GetComponent<MapComponent_AdrenalineTracker>().TryAddToCache(__instance);
            }
        }
    }

    [HarmonyPatch(typeof(Thing))]
    [HarmonyPatch(nameof(Thing.DeSpawn))]
    public static class Patch_DeSpawn
    {
        public static void Prefix(Thing __instance)
        {
            if (__instance.Map != null && __instance.IsPotentialPerceivableThreat())
            {
                __instance.Map.GetComponent<MapComponent_AdrenalineTracker>().TryRemoveFromCache(__instance);
            }
        }
    }
}