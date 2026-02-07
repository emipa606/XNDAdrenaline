using System.Reflection;
using Verse;

namespace Adrenaline;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    static HarmonyPatches()
    {
        Adrenaline.HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
    }
}