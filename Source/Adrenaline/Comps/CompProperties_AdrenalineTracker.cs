using Verse;

namespace Adrenaline;

public class CompProperties_AdrenalineTracker : CompProperties
{
    public readonly float adrenalineProductionCapacity = 10000;
    public readonly float adrenalineProductionRecoveryPerDay = 6400;

    public CompProperties_AdrenalineTracker()
    {
        compClass = typeof(CompAdrenalineTracker);
    }
}