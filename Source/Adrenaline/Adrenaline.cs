using UnityEngine;
using Verse;
using HarmonyLib;

namespace Adrenaline
{

    public class Adrenaline : Mod
    {

        public Adrenaline(ModContentPack content) : base(content)
        {
            GetSettings<AdrenalineSettings>();
            HarmonyInstance = new Harmony("XeoNovaDan.Adrenaline");
        }

        public static Harmony HarmonyInstance;

        public override string SettingsCategory() => "Adrenaline.SettingsCategory".Translate();

        public override void DoSettingsWindowContents(Rect inRect)
        {
            GetSettings<AdrenalineSettings>().DoWindowContents(inRect);
        }

    }

}
