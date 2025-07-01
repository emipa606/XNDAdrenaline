using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace Adrenaline;

public class Adrenaline : Mod
{
    public static Harmony HarmonyInstance;
    public static string CurrentVersion;

    public Adrenaline(ModContentPack content) : base(content)
    {
        GetSettings<AdrenalineSettings>();
        HarmonyInstance = new Harmony("XeoNovaDan.Adrenaline");
        CurrentVersion =
            VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
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