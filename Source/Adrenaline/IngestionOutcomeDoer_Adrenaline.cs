using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Adrenaline;

public class IngestionOutcomeDoer_Adrenaline : IngestionOutcomeDoer
{
    private readonly int adrenalineHediffDurationOffset;

    private readonly bool divideByBodySize;

    private readonly float severity = -1;

    public HediffDef hediffDef;

    // I definitely, definitely did not copy and paste a decompiled IngestionOutcomeDoer_GiveHediff and adapt it. Why would I ever do that?
    protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested)
    {
        // Check if the ingesting pawn can actually get adrenaline
        if (!pawn.CanGetAdrenaline())
        {
            return;
        }

        // Improperly configured properties
        if (hediffDef.hediffClass != typeof(Hediff_AdrenalineRush))
        {
            Log.Error(
                $"hediffDef for {ingested.def} does not have a hediffClass of Adrenaline.Hediff_AdrenalineRush");
            return;
        }

        // Do nothing if the pawn can't benefit from the ingested thing
        var extraRaceProps = pawn.def.GetModExtension<ExtendedRaceProperties>() ??
                             ExtendedRaceProperties.defaultValues;
        if (!extraRaceProps.RelevantConsumables.Contains(ingested.def))
        {
            return;
        }

        // Determine severity gain
        var severityGain = severity > 0 ? severity : hediffDef.initialSeverity;

        if (divideByBodySize)
        {
            severityGain /= pawn.BodySize;
        }

        severityGain *= extraRaceProps.adrenalineGainFactorArtificial;

        // Add severity and increase the duration of the hediff
        HealthUtility.AdjustSeverity(pawn, hediffDef, severityGain);
        var adrenalineHediff = (Hediff_AdrenalineRush)pawn.health.hediffSet.GetFirstHediffOfDef(hediffDef);
        adrenalineHediff.severityLossDelayTicks += adrenalineHediffDurationOffset;
    }

    public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
    {
        if (!parentDef.IsDrug || !(chance >= 1f))
        {
            yield break;
        }

        foreach (var s in hediffDef.SpecialDisplayStats(StatRequest.ForEmpty()))
        {
            yield return s;
        }
    }
}
// Okay, you got me... :(