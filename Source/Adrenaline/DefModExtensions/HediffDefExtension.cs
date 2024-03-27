﻿using System.Collections.Generic;
using Verse;

namespace Adrenaline;

public class HediffDefExtension : DefModExtension
{
    public static readonly HediffDefExtension defaultValues = new HediffDefExtension();

    public readonly AdrenalineCrashProperties adrenalineCrash = new AdrenalineCrashProperties();

    public readonly AdrenalineRushProperties adrenalineRush = new AdrenalineRushProperties();

    private readonly List<ExtraHediffStageProperties> stages;

    public ExtraHediffStageProperties GetExtraHediffStagePropertiesAt(int index)
    {
        return stages.NullOrEmpty() ? ExtraHediffStageProperties.defaultValues : stages[index];
    }
}