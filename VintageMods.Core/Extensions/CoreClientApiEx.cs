using Vintagestory.API.Client;

namespace VintageMods.Core.Extensions
{
    /// <summary>
    ///     Extension Methods for the Core Client API.
    /// </summary>
    public static class CoreClientApiEx
    {
        /// <summary>
        ///     Adds a waypoint at the player's current position within the world, relative to the global spawn point.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="icon">The icon to use for the waypoint.</param>
        /// <param name="colour">The colour of the waypoint.</param>
        /// <param name="title">The title to set.</param>
        /// <param name="pinned">if set to <c>true</c>, the waypoint will be pinned to the world map.</param>
        public static void AddWaypointAtCurrentPos(
            this ICoreClientAPI api, string icon, string colour, string title, bool pinned)
        {
            var blockPos = api.World?.Player?.Entity?.Pos.AsBlockPos.RelativeToSpawn(api);
            if (blockPos is null) return;
            api.SendChatMessage(
                $"/waypoint addati {icon} {blockPos.X} {blockPos.Y} {blockPos.Z} {(pinned ? "true" : "false")} {colour} {title}");
        }
    }
}