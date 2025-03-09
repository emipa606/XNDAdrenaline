using HarmonyLib;
using Verse;

namespace Adrenaline;

[HarmonyPatch(typeof(Thing), nameof(Thing.DeSpawn))]
public static class Thing_DeSpawn
{
    public static void Prefix(Thing __instance)
    {
        if (__instance.Map != null && __instance.IsPotentialPerceivableThreat())
        {
            __instance.Map.GetComponent<MapComponent_AdrenalineTracker>().TryRemoveFromCache(__instance);
        }
    }
}