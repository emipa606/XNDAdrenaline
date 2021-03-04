using System.Text;
using RimWorld;
using Verse;

namespace Adrenaline
{
    public class StatWorker_AdrenalineProduction : StatWorker
    {
        public override bool ShouldShowFor(StatRequest req)
        {
            return base.ShouldShowFor(req) && req.Def is ThingDef tDef && tDef.CanGetAdrenaline();
        }

        public override void FinalizeValue(StatRequest req, ref float val, bool applyPostProcess)
        {
            val *= ValueFactorFromRace(req.Def) * ValueFactorFromTracker(req.Thing);
            base.FinalizeValue(req, ref val, applyPostProcess);
        }

        public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense,
            float finalVal)
        {
            var explanationBuilder = new StringBuilder();
            explanationBuilder.AppendLine(
                $"{req.Def.LabelCap}: {ValueFactorFromRace(req.Def).ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Factor)}");
            explanationBuilder.AppendLine(
                $"{"Adrenaline.StatsReport_RecentlyProducedAdrenaline".Translate()}: {ValueFactorFromTracker(req.Thing).ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Factor)}");
            explanationBuilder.AppendLine();
            explanationBuilder.AppendLine(base.GetExplanationFinalizePart(req, numberSense, finalVal));
            return explanationBuilder.ToString();
        }

        private float ValueFactorFromRace(Def def)
        {
            var extraRaceProps = def.GetModExtension<ExtendedRaceProperties>() ?? ExtendedRaceProperties.defaultValues;
            return extraRaceProps.adrenalineGainFactorNatural;
        }

        private float ValueFactorFromTracker(Thing thing)
        {
            var adrenalineTracker = thing.TryGetComp<CompAdrenalineTracker>();
            if (adrenalineTracker != null)
            {
                return adrenalineTracker.AdrenalineProductionFactor;
            }

            // Null adrenaline tracker
            if (Current.ProgramState == ProgramState.Playing)
            {
                Log.Error($"Tried to get factor from CompAdrenalineTracker for {thing} but adrenalineTracker is null");
            }

            return 1;
        }
    }
}