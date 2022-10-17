using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace Adrenaline;

public class Adrenaline : Mod
{
    public static Harmony HarmonyInstance;
    public static string currentVersion;

    public Adrenaline(ModContentPack content) : base(content)
    {
        GetSettings<AdrenalineSettings>();
        HarmonyInstance = new Harmony("XeoNovaDan.Adrenaline");
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(ModLister.GetActiveModWithIdentifier("Mlie.XNDAdrenaline"));
    }

    public override string SettingsCategory()
    {
        return "Adrenaline.SettingsCategory".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        GetSettings<AdrenalineSettings>().DoWindowContents(inRect);
    }
}