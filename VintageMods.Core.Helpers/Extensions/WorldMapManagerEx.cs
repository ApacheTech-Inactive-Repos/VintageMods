using System.Linq;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace VintageMods.Core.Helpers.Extensions
{
    public static class WorldMapManagerEx
    {
        public static WaypointMapLayer WaypointMapLayer(this WorldMapManager mapManager)
        {
            return mapManager.MapLayers.OfType<WaypointMapLayer>().FirstOrDefault();
        }

        public static void AddWaypointToPlayer(this WorldMapManager mapManager, Waypoint waypoint, IServerPlayer player)
        {
            var waypointMapLayer = mapManager.WaypointMapLayer();
            waypointMapLayer?.Waypoints.Add(waypoint);
            mapManager.ResendWaypoints(player, waypointMapLayer);
        }

        public static void ResendWaypoints(this WorldMapManager mapManager, IServerPlayer player, WaypointMapLayer mapLayer)
        {
            var playerGroupMemberships = player.ServerData.PlayerGroupMemberships;

            var list = mapLayer.Waypoints.Where(waypoint => 
                    player.PlayerUID == waypoint.OwningPlayerUid || 
                    playerGroupMemberships.ContainsKey(waypoint.OwningPlayerGroupId))
                .ToList();

            mapManager.SendMapDataToClient(mapLayer, player, SerializerUtil.Serialize(list));
        }
    }
}