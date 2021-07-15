using System.Linq;
using RimWorld;
using Verse;

namespace Adrenaline
{
    public class HediffGiver_Adrenaline : HediffGiver
    {
        public override void OnIntervalPassed(Pawn pawn, Hediff cause)
        {
            var extraRaceProps = pawn.def.GetModExtension<ExtendedRaceProperties>() ??
                                 ExtendedRaceProperties.defaultValues;

            if (!extraRaceProps.HasAdrenaline)
            {
                return;
            }

            var adrenalineTracker = pawn.GetComp<CompAdrenalineTracker>();
            var hasRush = pawn.health.hediffSet.HasHediff(extraRaceProps.adrenalineRushHediff);

            // If the pawn can produce adrenaline and doesn't already have an adrenaline rush, add adrenaline rush
            if (AdrenalineSettings.allowNaturalGain &&
                (AdrenalineSettings.affectAnimals || !pawn.RaceProps.Animal) &&
                adrenalineTracker.CanProduceAdrenaline && !hasRush &&
                AdrenalineUtility.GetPerceivedThreatsFor(pawn).Any())
            {
                TryTeachAdrenalineConcept(pawn);
                pawn.health.AddHediff(extraRaceProps.adrenalineRushHediff);
            }

            // Otherwise if they have an adrenaline rush and don't have an adrenaline crash hediff, add an adrenaline crash hediff
            else if (AdrenalineSettings.adrenalineCrashes && hasRush &&
                     extraRaceProps.adrenalineCrashHediff != null &&
                     !pawn.health.hediffSet.HasHediff(extraRaceProps.adrenalineCrashHediff))
            {
                var crashHediff =
                    (Hediff_AdrenalineCrash) pawn.health.AddHediff(extraRaceProps.adrenalineCrashHediff);
                crashHediff.ticksToSeverityGain = crashHediff.Props.severityGainDelay;
            }
        }

        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            if (!AdrenalineSettings.allowNaturalGain || !AdrenalineSettings.affectAnimals && pawn.RaceProps.Animal)
            {
                return false;
            }

            var extraRaceProps = pawn.def.GetModExtension<ExtendedRaceProperties>() ??
                                 ExtendedRaceProperties.defaultValues;
            if (!extraRaceProps.HasAdrenaline)
            {
                return false;
            }

            var adrenalineTracker = pawn.GetComp<CompAdrenalineTracker>();
            if (!adrenalineTracker.CanProduceAdrenaline)
            {
                return false;
            }

            // Hediff isn't an injury, is a scar or the pawn is dead
            if (!(hediff is Hediff_Injury injury) || injury.IsPermanent() || pawn.Dead)
            {
                return false;
            }

            // Try to add target severity based on the pain caused by the injury
            var painFromInjury = injury.PainOffset / pawn.HealthScale * pawn.TotalPainFactor();
            if (!(painFromInjury > 0))
            {
                return false;
            }

            TryTeachAdrenalineConcept(pawn);
            var rushhediff =
                (Hediff_AdrenalineRush) (pawn.health.hediffSet.GetFirstHediffOfDef(extraRaceProps
                                             .adrenalineRushHediff) ??
                                         pawn.health.AddHediff(extraRaceProps.adrenalineRushHediff));
            rushhediff.recentPainFelt += painFromInjury;
            return true;
        }

        private void TryTeachAdrenalineConcept(Pawn pawn)
        {
            if (pawn.Faction == Faction.OfPlayer && !PlayerKnowledgeDatabase.IsComplete(A_ConceptDefOf.Adrenaline))
            {
                LessonAutoActivator.TeachOpportunity(A_ConceptDefOf.Adrenaline, OpportunityType.Important);
            }
        }
    }
}