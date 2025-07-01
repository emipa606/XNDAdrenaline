using System.Text;
using RimWorld;
using Verse;

namespace Adrenaline;

public class StatWorker_AdrenalineProduction : StatWorker
{
    public override bool ShouldShowFor(StatRequest req)
    {
        return base.ShouldShowFor(req) && req.Def is ThingDef tDef && tDef.CanGetAdrenaline();
    }

    public override void FinalizeValue(StatRequest req, ref float val, bool applyPostProcess)
    {
        val *= valueFactorFromRace(req.Def) * valueFactorFromTracker(req.Thing);
        base.FinalizeValue(req, ref val, applyPostProcess);
    }

    public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense,
        float finalVal)
    {
        var explanationBuilder = new StringBuilder();
        explanationBuilder.AppendLine(
            $"{req.Def.LabelCap}: {valueFactorFromRace(req.Def).ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Factor)}");
        explanationBuilder.AppendLine(
            $"{"Adrenaline.StatsReport_RecentlyProducedAdrenaline".Translate()}: {valueFactorFromTracker(req.Thing).ToStringByStyle(stat.toStringStyle, ToStringNumberSense.Factor)}");
        explanationBuilder.AppendLine();
        explanationBuilder.AppendLine(base.GetExplanationFinalizePart(req, numberSense, finalVal));
        return explanationBuilder.ToString();
    }

    private static float valueFactorFromRace(Def def)
    {
        var extraRaceProps = def.GetModExtension<ExtendedRaceProperties>() ?? ExtendedRaceProperties.defaultValues;
        return extraRaceProps.adrenalineGainFactorNatural;
    }

    private static float valueFactorFromTracker(Thing thing)
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