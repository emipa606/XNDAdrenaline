using System.Collections.Generic;
using Verse;

namespace Adrenaline
{

    public class HediffDefExtension : DefModExtension
    {

        public static readonly HediffDefExtension defaultValues = new HediffDefExtension();

        public ExtraHediffStageProperties GetExtraHediffStagePropertiesAt(int index)
        {
            return stages.NullOrEmpty() ? ExtraHediffStageProperties.defaultValues : stages[index];
        }

        private readonly List<ExtraHediffStageProperties> stages;

        public AdrenalineRushProperties adrenalineRush = new AdrenalineRushProperties();

        public AdrenalineCrashProperties adrenalineCrash = new AdrenalineCrashProperties();

    }

}
