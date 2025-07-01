using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Adrenaline;

public class MapComponent_AdrenalineTracker : MapComponent
{
    private const int AllPotentialHostileThingsUpdateInterval = 120;

    private readonly HashSet<Thing> cachedPotentialHostileThings = [];

    public HashSet<Thing> allPotentialHostileThings = [];

    private bool cacheSet;

    public MapComponent_AdrenalineTracker(Map map) : base(map)
    {
    }

    public override void MapComponentTick()
    {
        // For save compatibility, but also because getting the list to save is a PITA
        if (!cacheSet)
        {
            resetCachedPotentialHostileThings();
            cacheSet = true;
        }

        if (Find.TickManager.TicksGame % AllPotentialHostileThingsUpdateInterval == 0)
        {
            allPotentialHostileThings =
                [..map.mapPawns.AllPawnsSpawned.ToArray().Concat(cachedPotentialHostileThings)];
        }
    }

    public void TryAddToCache(Thing t)
    {
        cachedPotentialHostileThings.Add(t);
    }

    public void TryRemoveFromCache(Thing t)
    {
        if (cachedPotentialHostileThings.Contains(t))
        {
            cachedPotentialHostileThings.Remove(t);
        }
    }

    private void resetCachedPotentialHostileThings()
    {
        cachedPotentialHostileThings.Clear();
        foreach (var thing in map.listerThings.AllThings)
        {
            if (thing.IsPotentialPerceivableThreat())
            {
                TryAddToCache(thing);
            }
        }
    }
}