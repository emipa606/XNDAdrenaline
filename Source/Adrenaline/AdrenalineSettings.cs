using UnityEngine;
using Verse;

namespace Adrenaline;

public class AdrenalineSettings : ModSettings
{
    public static bool AllowNaturalGain = true;
    public static bool AffectAnimals = true;
    public static bool AffectDownedPawns;
    public static bool AdrenalineCrashes = true;
    public static bool NpcUse = true;

    public void DoWindowContents(Rect wrect)
    {
        var options = new Listing_Standard();
        var defaultColor = GUI.color;
        options.Begin(wrect);
        GUI.color = defaultColor;
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;

        // Natural gain
        options.Gap();
        options.CheckboxLabeled("Adrenaline.AllowNaturalGain".Translate(), ref AllowNaturalGain,
            "Adrenaline.AllowNaturalGain_ToolTip".Translate());

        // Affect animals
        if (!AllowNaturalGain)
        {
            GUI.color = Color.grey;
        }

        options.Gap();
        options.CheckboxLabeled("Adrenaline.AffectAnimals".Translate(), ref AffectAnimals,
            "Adrenaline.AffectAnimals_ToolTip".Translate());
        GUI.color = defaultColor;

        // Downed pawns
        options.Gap();
        options.CheckboxLabeled("Adrenaline.AffectDownedPawns".Translate(), ref AffectDownedPawns,
            "Adrenaline.AffectDownedPawns_ToolTip".Translate());

        // Adrenaline crashes
        options.Gap();
        options.CheckboxLabeled("Adrenaline.AllowAdrenalineCrashes".Translate(), ref AdrenalineCrashes,
            "Adrenaline.AllowAdrenalineCrashes_ToolTip".Translate());

        // NPCs
        options.Gap();
        options.CheckboxLabeled("Adrenaline.AllowNPCUse".Translate(), ref NpcUse,
            "Adrenaline.AllowNPCUse_ToolTip".Translate());
        if (Adrenaline.CurrentVersion != null)
        {
            options.Gap();
            GUI.contentColor = Color.gray;
            options.Label("Adrenaline.CurrentModVersion".Translate(Adrenaline.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        options.End();
        Mod.GetSettings<AdrenalineSettings>().Write();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref AllowNaturalGain, "allowNaturalGain", true);
        Scribe_Values.Look(ref AffectAnimals, "affectAnimals", true);
        Scribe_Values.Look(ref AffectDownedPawns, "affectDownedPawns");
        Scribe_Values.Look(ref AdrenalineCrashes, "adrenalineCrashes", true);
        Scribe_Values.Look(ref NpcUse, "npcUse", true);
    }
}