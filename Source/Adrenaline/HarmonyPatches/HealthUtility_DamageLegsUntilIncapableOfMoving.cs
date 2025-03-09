using HarmonyLib;
using Verse;

namespace Adrenaline;

[HarmonyPatch(typeof(HealthUtility), nameof(HealthUtility.DamageLegsUntilIncapableOfMoving))]
public static class HealthUtility_DamageLegsUntilIncapableOfMoving
{
    public static void Prefix(Pawn p)
    {
        HealthUtility_DamageUntilDowned.Prefix(p);
    }
}