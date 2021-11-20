using ProtoBuf;

namespace ApacheTech.WaypointExtensions.Mod.Model
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class WorldSettings
    {
        public bool AutoTranslocatorWaypoints { get; set; }

        public bool AutoTraderWaypoints { get; set; }
    }
}