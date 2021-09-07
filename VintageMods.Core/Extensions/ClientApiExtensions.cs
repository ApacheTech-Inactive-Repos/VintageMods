using System.Collections.Generic;
using System.Linq;
using VintageMods.Core.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using Vintagestory.Server;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace VintageMods.Core.Extensions
{
    public static class ClientApiExtensions
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

        public static ServerMain AsServerMain(this ICoreServerAPI api)
        {
            return api.World as ServerMain;
        }

        public static ICoreClientAPI AsApi(this ClientMain game)
        {
            return game.Api as ICoreClientAPI;
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

        public static TBlockEntity GetNearestBlockEntity<TBlockEntity>(this IWorldAccessor world, BlockPos pos, 
            float horRange, float vertRange, Func<TBlockEntity, bool> predicate) where TBlockEntity : BlockEntity
        {
            TBlockEntity blockEntity = null;
            var minPos = pos.AddCopy(-horRange, -vertRange, -horRange);
            var maxPos = pos.AddCopy(horRange, vertRange, horRange);
            world.BlockAccessor.WalkBlocks(minPos, maxPos, (_, blockPos) =>
            {
                var entity = world.BlockAccessor.GetBlockEntity(blockPos);
                if (entity == null) return;
                if (entity.GetType() == typeof(TBlockEntity) && predicate((TBlockEntity)entity))
                {
                    blockEntity = (TBlockEntity)entity;
                }
            }, true);
            return blockEntity;
        }

        public static TBlockEntity GetNearestBlockEntity<TBlockEntity>(this IWorldAccessor world, BlockPos pos, 
            float horRange, float vertRange) where TBlockEntity : BlockEntity
        {
            return world.GetNearestBlockEntity<TBlockEntity>(pos, horRange, vertRange, _ => true);
        }
        
        /// <summary>
        ///     Determines whether any waypoints exist within a given radius of a block position.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="pos">The position to check for waypoints around.</param>
        /// <param name="radius">The radius around the origin position.</param>
        /// <param name="comparer">Optional parameters to narrow down waypoint scanning.</param>
        /// <returns><c>true</c> if any waypoints are found, <c>false</c> otherwise.</returns>
        public static bool WaypointExistsWithinRadius(this ICoreClientAPI api, BlockPos pos, int horizontalRadius, int verticalRadius, Func<Waypoint, bool> comparer = null)
        {
            var waypointMapLayer = api.ModLoader.GetModSystem<WorldMapManager>().WaypointMapLayer();
            var waypoints =
                waypointMapLayer.ownWaypoints.Where(wp =>
                    wp.Position.AsBlockPos.InRangeCubic(pos, horizontalRadius, verticalRadius)).ToList();
            if (!waypoints.Any()) return false;
            return comparer == null || waypoints.Any(p => comparer(p));
        }

        public static bool InRangeCubic(this BlockPos pos, BlockPos relativeToBlock, int horizontalRadius = 10, int verticalRadius = 10)
        {
            if (!pos.InRangeHorizontally(relativeToBlock.X, relativeToBlock.Z, horizontalRadius)) return false;
            return pos.Y <= relativeToBlock.Y + verticalRadius && pos.Y >= relativeToBlock.Y - verticalRadius;
        }
        
        /// <summary>
        ///     Determines whether any waypoints exist at a specific block position.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="pos">The position to check for waypoints at.</param>
        /// <param name="comparer">Optional parameters to narrow down waypoint scanning.</param>
        /// <returns><c>true</c> if any waypoints are found, <c>false</c> otherwise.</returns>
        public static bool WaypointExistsAtPos(this ICoreClientAPI api, BlockPos pos, Func<Waypoint, bool> comparer = null)
        {
            var waypointMapLayer = api.ModLoader.GetModSystem<WorldMapManager>().WaypointMapLayer();
            var waypoints = waypointMapLayer.ownWaypoints.Where(wp => wp.Position.AsBlockPos.Equals(pos)).ToList();
            if (!waypoints.Any()) return false;
            return comparer == null || waypoints.Any(p => comparer(p));
        }

        /// <summary>
        ///     Gets the position relative to spawn, given an absolute position within the game world.
        /// </summary>
        /// <param name="pos">The absolute position of the block being queried.</param>
        /// <param name="world">The API to use for game world information.</param>
        public static BlockPos RelativeToSpawn(this BlockPos pos, IWorldAccessor world)
        {
            var blockPos = pos.SubCopy(world.DefaultSpawnPosition.XYZ.AsBlockPos);
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
            api.AddWaypointAtPos(api.World?.Player?.Entity?.Pos.AsBlockPos.RelativeToSpawn(api.World), icon, colour, title, pinned);
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
            if (pos is null) return;
            api.SendChatMessage(
                $"/waypoint addati {icon} {pos.X} {pos.Y} {pos.Z} {(pinned ? "true" : "false")} {colour} {title}");
        }
    }
}