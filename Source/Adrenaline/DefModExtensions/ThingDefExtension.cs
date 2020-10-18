using Verse;

namespace Adrenaline
{

    public class ThingDefExtension : DefModExtension
    {

        public static readonly ThingDefExtension defaultValues = new ThingDefExtension();

        public bool ingestibleWhenDowned;
        public string downedIngestGizmoLabel;
        public string downedIngestGizmoDescription;
        public string downedIngestGizmoTexPath;
        public string downedIngestGizmoNoneNearby;

    }

}
