using System.Collections.Generic;
using System.Linq;
using VintageMods.Core.Common.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace VintageMods.Core.Client.Extensions
{
    public static class ClientApiExtenstions
    {
        /// <summary>
        ///     Gets the world seed.
        /// </summary>
        /// <param name="api">The core game API.</param>
        public static string GetSeed(this ICoreAPI api)
        {
            return api?.World?.Seed.ToString();
        }

        public static ClientMain AsClientMain(this ICoreClientAPI api)
        {
            return api.World as ClientMain;
        }
        public static object GetVanillaClientSystem(this ICoreClientAPI api, string name)
        {
            var clientSystems = (api.World as ClientMain).GetField<ClientSystem[]>("clientSystems");
            return clientSystems.FirstOrDefault(p => p.Name == name);
        }
        public static void UnregisterCommand(this ICoreClientAPI capi, string cmd)
        {
            var eventManager = (capi.World as ClientMain).GetField<ClientEventManager>("eventManager");
            var chatCommands = eventManager.GetField<Dictionary<string, ChatCommand>>("chatCommands");
            if (chatCommands.ContainsKey(cmd))
            {
                chatCommands.Remove(cmd);
            }
            eventManager.SetField("chatCommands", chatCommands);
        }
        public static void UnregisterVanillaClientSystem<T>(this ICoreClientAPI capi) where T : ClientSystem
        {
            var clientMain = capi.World as ClientMain;
            var clientSystems = clientMain.GetField<ClientSystem[]>("clientSystems").ToList();
            for (var i = 0; i < clientSystems.Count; i++)
            {
                if (clientSystems[i].GetType() != typeof(T)) continue;
                clientSystems[i].Dispose(clientMain);
                clientSystems.Remove(clientSystems[i]);
                break;
            }
            clientMain.SetField("clientSystems", clientSystems.ToArray());
        }
        public static void UnregisterVanillaClientSystem(this ICoreClientAPI capi, string name)
        {
            var clientMain = capi.World as ClientMain;
            var clientSystems = clientMain.GetField<ClientSystem[]>("clientSystems").ToList();
            for (var i = 0; i < clientSystems.Count; i++)
            {
                if (clientSystems[i].Name != name) continue;
                clientSystems[i].Dispose(clientMain);
                clientSystems.Remove(clientSystems[i]);
                break;
            }
            clientMain.SetField("clientSystems", clientSystems.ToArray());
        }

        public static T GetVanillaClientSystem<T>(this ICoreClientAPI api) where T : ClientSystem
        {
            var clientSystems = (api.World as ClientMain).GetField<ClientSystem[]>("clientSystems");
            return clientSystems.FirstOrDefault(p => p.GetType() == typeof(T)) as T;
        }

        /// <summary>
        ///     Deletes waypoints.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="icon"></param>
        public static void PurgeWaypointsByIcon(this ICoreClientAPI api, string icon)
        {
            var wpLayer = api.ModLoader.GetModSystem<WorldMapManager>().WaypointMapLayer();
            int ScanWaypoints()
            {
                for (var i = 0; i < wpLayer.ownWaypoints.Count; i++)
                {
                    var wp = wpLayer.ownWaypoints[i];
                    if (wp.Icon != icon) continue;
                    return i;
                }
                return -1;
            }

            TyronThreadPool.QueueTask(() =>
            {
                var index = ScanWaypoints();
                while (index < -1)
                {
                    var i = index;
                    api.AsClientMain().EnqueueMainThreadTask(() =>
                        api.SendChatMessage($"/waypoint remove {i}"), "");
                    index = ScanWaypoints();
                }
            });
        }

        /// <summary>
        ///     Thread-Safe.
        ///     Shows a client side only chat message in the current chat channel. Does not execute client commands.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="message">The message to show to the player.</param>
        public static void EnqueueShowChatMessage(this ICoreClientAPI api, string message)
        {
            (api.World as ClientMain)?.EnqueueShowChatMessage(message);
        }

        /// <summary>
        ///     Thread-Safe.
        ///     Shows a client side only chat message in the current chat channel. Does not execute client commands.
        /// </summary>
        /// <param name="game">The core game API this method was called from.</param>
        /// <param name="message">The message to show to the player.</param>
        public static void EnqueueShowChatMessage(this ClientMain game, string message)
        {

            game?.EnqueueMainThreadTask(() => game.ShowChatMessage(message), "");
        }

        /// <summary>
        ///     Returns the map layer used for rendering waypoints.
        /// </summary>
        /// <param name="mapManager">The <see cref="WorldMapManager"/> instance that this method was called from.</param>
        public static WaypointMapLayer WaypointMapLayer(this WorldMapManager mapManager)
        {
            return mapManager.MapLayers.OfType<WaypointMapLayer>().FirstOrDefault();
        }

        /// <summary>
        ///     Determines whether any waypoints exist within a given radius of a block position.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="pos">The position to check for waypoints around.</param>
        /// <param name="radius">The radius around the origin position.</param>
        /// <param name="icon"></param>
        /// <param name="partialTitle"></param>
        /// <returns><c>true</c> if any waypoints are found, <c>false</c> otherwise.</returns>
        public static bool WaypointExistsWithinRadius(this ICoreClientAPI api, BlockPos pos, int radius, string icon, string partialTitle = null)
        {
            var waypointMapLayer = api.ModLoader.GetModSystem<WorldMapManager>().WaypointMapLayer();
            foreach (var wp in waypointMapLayer.ownWaypoints.Where(wp => wp.Icon == icon).Where(wp => wp.Position.AsBlockPos.InRangeHorizontally(pos.X, pos.Z, radius)))
            {
                if (string.IsNullOrWhiteSpace(partialTitle)) return true;
                if (wp.Title.Contains(partialTitle))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Gets the position relative to spawn, given an absolute position within the game world.
        /// </summary>
        /// <param name="pos">The absolute position of the block being queried.</param>
        /// <param name="api">The API to use for game world information.</param>
        public static BlockPos RelativeToSpawn(this BlockPos pos, ICoreClientAPI api)
        {
            var blockPos = pos.SubCopy(api.World.DefaultSpawnPosition.XYZ.AsBlockPos);
            return new BlockPos(blockPos.X, pos.Y, blockPos.Z);
        }

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
            api.AddWaypointAtPos(api.World?.Player?.Entity?.Pos.AsBlockPos.RelativeToSpawn(api), icon, colour, title, pinned);
        }

        /// <summary>
        ///     Adds a waypoint at the a position within the world, relative to the global spawn point.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="pos">The position to add the waypoint at.</param>
        /// <param name="icon">The icon to use for the waypoint.</param>
        /// <param name="colour">The colour of the waypoint.</param>
        /// <param name="title">The title to set.</param>
        /// <param name="pinned">if set to <c>true</c>, the waypoint will be pinned to the world map.</param>
        public static void AddWaypointAtPos(
            this ICoreClientAPI api, BlockPos pos, string icon, string colour, string title, bool pinned)
        {
            var blockPos = pos;
            if (blockPos is null) return;
            api.SendChatMessage(
                $"/waypoint addati {icon} {blockPos.X} {blockPos.Y} {blockPos.Z} {(pinned ? "true" : "false")} {colour} {title}");
        }
    }
}