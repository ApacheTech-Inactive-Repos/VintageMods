using System.Linq;
using Vintagestory.GameContent;

namespace VintageMods.Core.Common.Extensions
{
    /// <summary>
    ///     Extension Methods for the World Map Manager.
    /// </summary>
    public static class WorldMapManagerEx
    {
        /// <summary>
        ///     Returns the map layer used for rendering waypoints.
        /// </summary>
        /// <param name="mapManager">The <see cref="WorldMapManager"/> instance that this method was called from.</param>
        public static WaypointMapLayer WaypointMapLayer(this WorldMapManager mapManager)
        {
            return mapManager.MapLayers.OfType<WaypointMapLayer>().FirstOrDefault();
        }
    }
}