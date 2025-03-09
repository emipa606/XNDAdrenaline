using HarmonyLib;
using Verse;

namespace Adrenaline;

[HarmonyPatch(typeof(Thing), nameof(Thing.SpawnSetup))]
public static class Thing_SpawnSetup
{
    public static void Postfix(Thing __instance)
    {
        if (__instance.Map != null && __instance.IsPotentialPerceivableThreat())
        {
            __instance.Map.GetComponent<MapComponent_AdrenalineTracker>().TryAddToCache(__instance);
        }
    }
}