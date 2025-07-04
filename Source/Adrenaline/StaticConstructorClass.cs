﻿using UnityEngine;
using Verse;

namespace Adrenaline;

[StaticConstructorOnStartup]
public static class StaticConstructorClass
{
    static StaticConstructorClass()
    {
        foreach (var tDef in DefDatabase<ThingDef>.AllDefs)
        {
            // Add CompAdrenalineTracker to each eligible pawn def that doesn't already have one
            if (tDef.CanGetAdrenaline())
            {
                tDef.comps ??= [];

                if (!tDef.comps.Any(c => c.GetType() == typeof(CompProperties_AdrenalineTracker)))
                {
                    tDef.comps.Add(new CompProperties_AdrenalineTracker());
                }
            }

            // Populate adrenaline gizmo icons
            else if (tDef.GetModExtension<ThingDefExtension>() is { } thingDefExtension &&
                     !thingDefExtension.downedIngestGizmoTexPath.NullOrEmpty())
            {
                AdrenalineUtility.AdrenalineGizmoIcons.Add(tDef,
                    ContentFinder<Texture2D>.Get(thingDefExtension.downedIngestGizmoTexPath));
            }
        }
    }
}