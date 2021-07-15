using Verse;

namespace Adrenaline
{
    public class ThingDefExtension : DefModExtension
    {
        public static readonly ThingDefExtension defaultValues = new ThingDefExtension();
        public string downedIngestGizmoDescription;
        public string downedIngestGizmoLabel;
        public string downedIngestGizmoNoneNearby;
        public string downedIngestGizmoTexPath;

        public bool ingestibleWhenDowned;
    }
}