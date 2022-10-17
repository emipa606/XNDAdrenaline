﻿using UnityEngine;
using Verse;

namespace Adrenaline;

public class AdrenalineSettings : ModSettings
{
    public static bool allowNaturalGain = true;
    public static bool affectAnimals = true;
    public static bool affectDownedPawns;
    public static bool adrenalineCrashes = true;
    public static bool npcUse = true;

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
        options.CheckboxLabeled("Adrenaline.AllowNaturalGain".Translate(), ref allowNaturalGain,
            "Adrenaline.AllowNaturalGain_ToolTip".Translate());

        // Affect animals
        if (!allowNaturalGain)
        {
            GUI.color = Color.grey;
        }

        options.Gap();
        options.CheckboxLabeled("Adrenaline.AffectAnimals".Translate(), ref affectAnimals,
            "Adrenaline.AffectAnimals_ToolTip".Translate());
        GUI.color = defaultColor;

        // Downed pawns
        options.Gap();
        options.CheckboxLabeled("Adrenaline.AffectDownedPawns".Translate(), ref affectDownedPawns,
            "Adrenaline.AffectDownedPawns_ToolTip".Translate());

        // Adrenaline crashes
        options.Gap();
        options.CheckboxLabeled("Adrenaline.AllowAdrenalineCrashes".Translate(), ref adrenalineCrashes,
            "Adrenaline.AllowAdrenalineCrashes_ToolTip".Translate());

        // NPCs
        options.Gap();
        options.CheckboxLabeled("Adrenaline.AllowNPCUse".Translate(), ref npcUse,
            "Adrenaline.AllowNPCUse_ToolTip".Translate());
        if (Adrenaline.currentVersion != null)
        {
            options.Gap();
            GUI.contentColor = Color.gray;
            options.Label("Adrenaline.CurrentModVersion".Translate(Adrenaline.currentVersion));
            GUI.contentColor = Color.white;
        }

        options.End();
        Mod.GetSettings<AdrenalineSettings>().Write();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref allowNaturalGain, "allowNaturalGain", true);
        Scribe_Values.Look(ref affectAnimals, "affectAnimals", true);
        Scribe_Values.Look(ref affectDownedPawns, "affectDownedPawns");
        Scribe_Values.Look(ref adrenalineCrashes, "adrenalineCrashes", true);
        Scribe_Values.Look(ref npcUse, "npcUse", true);
    }
}