using Verse;

namespace Adrenaline
{
    public class CompProperties_AdrenalineTracker : CompProperties
    {
        public float adrenalineProductionCapacity = 10000;
        public float adrenalineProductionRecoveryPerDay = 6400;

        public CompProperties_AdrenalineTracker()
        {
            compClass = typeof(CompAdrenalineTracker);
        }
    }
}