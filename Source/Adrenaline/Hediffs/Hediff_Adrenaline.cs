using Verse;

namespace Adrenaline
{

    public abstract class Hediff_Adrenaline : HediffWithComps
    {

        protected const int SeverityUpdateIntervalTicks = 20;

        protected ExtendedRaceProperties ExtraRaceProps => pawn.def.GetModExtension<ExtendedRaceProperties>() ?? ExtendedRaceProperties.defaultValues;

        protected CompAdrenalineTracker AdrenalineTracker => pawn.GetComp<CompAdrenalineTracker>();

        protected abstract void UpdateSeverity();

        public override void Tick()
        {
            if (ageTicks % SeverityUpdateIntervalTicks == 0)
                UpdateSeverity();

            base.Tick();
        }

    }

}
