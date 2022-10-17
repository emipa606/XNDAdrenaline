using System.Collections.Generic;
using Verse;

namespace Adrenaline;

public class HediffDefExtension : DefModExtension
{
    public static readonly HediffDefExtension defaultValues = new HediffDefExtension();

    private readonly List<ExtraHediffStageProperties> stages;

    public AdrenalineCrashProperties adrenalineCrash = new AdrenalineCrashProperties();

    public AdrenalineRushProperties adrenalineRush = new AdrenalineRushProperties();

    public ExtraHediffStageProperties GetExtraHediffStagePropertiesAt(int index)
    {
        return stages.NullOrEmpty() ? ExtraHediffStageProperties.defaultValues : stages[index];
    }
}