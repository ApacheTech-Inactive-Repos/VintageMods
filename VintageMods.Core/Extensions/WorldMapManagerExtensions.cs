using System.Linq;
using JetBrains.Annotations;
using VintageMods.Core.Reflection;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

namespace VintageMods.Core.Extensions
{
    /// <summary>
    ///     Extension Methods for the World Map Manager.
    /// </summary>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public static class WorldMapManagerExtensions
    {
        /// <summary>
        ///     Returns the map layer used for rendering waypoints.
        /// </summary>
        /// <param name="mapManager">The <see cref="WorldMapManager" /> instance that this method was called from.</param>
        public static WaypointMapLayer WaypointMapLayer(this WorldMapManager mapManager)
        {
            return mapManager.MapLayers.OfType<WaypointMapLayer>().FirstOrDefault();
        }

        /// <summary>
        ///     Trick the server into sending waypoints to the client even if they don't have their map opened.
        ///     Credit to Novocain.
        /// </summary>
        /// <param name="mapManager">The map manager.</param>
        public static void ForceSendWaypoints(this WorldMapManager mapManager)
        {
            var capi = mapManager.GetField<ICoreClientAPI>("capi");
            capi.Event.EnqueueMainThreadTask(() =>
                capi.Event.RegisterCallback(dt =>
                    mapManager.GetField<IClientNetworkChannel>("clientChannel")
                        .SendPacket(new OnViewChangedPacket()), 500), "");
        }
    }
}