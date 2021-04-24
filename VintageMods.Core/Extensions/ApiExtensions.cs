using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VintageMods.Core.Attributes;
using VintageMods.Core.FileIO;
using VintageMods.Core.FileIO.Enum;
using VintageMods.Core.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedParameter.Global

namespace VintageMods.Core.Extensions
{
    public static class ApiExtensions
    {
        private static FileManager _fileManagerInstance;

        public static FileManager RegisterModFiles(this ICoreAPI api,
            params (string FileName, FileType FileType, FileScope FileScope)[] files)
        {
            var rootFolder = Assembly.GetCallingAssembly().GetCustomAttributes().OfType<ModDomainAttribute>().FirstOrDefault()?.RootFolder ?? "VintageMods";
            _fileManagerInstance = new FileManager(api, rootFolder);
            foreach (var (fileName, fileType, fileScope) in files)
                _fileManagerInstance.RegisterFile(fileName, fileType, fileScope);

            return _fileManagerInstance;
        }

        public static FileManager FileManager(this ICoreAPI api, string rootFolder = null)
        {
            if (string.IsNullOrEmpty(rootFolder)) return _fileManagerInstance;
            return _fileManagerInstance ??= new FileManager(api, rootFolder);
        }

        public static ModFileInfo RegisterModConfigFile(this ICoreAPI api, string fileName, FileScope fileScope)
        {
            try
            {
                return _fileManagerInstance.RegisterConfigFile(fileName, fileScope);
            }
            catch (Exception e)
            {
                api.Logger.Error(e.Message);
                throw;
            }
        }

        public static ModFileInfo RegisterModDataFile(this ICoreAPI api, string fileName, FileScope fileScope)
        {
            try
            {
                return _fileManagerInstance.RegisterDataFile(fileName, fileScope);
            }
            catch (Exception e)
            {
                api.Logger.Error(e.Message);
                throw;
            }
        }

        public static ModFileInfo GetModFile(this ICoreAPI api, string fileName)
        {
            try
            {
                return _fileManagerInstance.ModFiles[fileName];
            }
            catch (Exception e)
            {
                api.Logger.Error(e.Message);
                throw;
            }
        }

        private static string _modDomain = "";

        public static void RegisterModDomain(this ICoreClientAPI api, string domain)
        {
            _modDomain = domain;
        }

        public static string ModDomain(this ICoreClientAPI api)
        {
            return _modDomain;
        }

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
            if (chatCommands.ContainsKey(cmd)) chatCommands.Remove(cmd);
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


        /// <summary>
        ///     Determines whether any waypoints exist within a given radius of a block position.
        /// </summary>
        /// <param name="api">The core game API this method was called from.</param>
        /// <param name="pos">The position to check for waypoints around.</param>
        /// <param name="radius">The radius around the origin position.</param>
        /// <param name="icon"></param>
        /// <param name="partialTitle"></param>
        /// <returns><c>true</c> if any waypoints are found, <c>false</c> otherwise.</returns>
        public static bool WaypointExistsWithinRadius(this ICoreClientAPI api, BlockPos pos, int radius, string icon,
            string partialTitle = null)
        {
            var waypointMapLayer = api.ModLoader.GetModSystem<WorldMapManager>().WaypointMapLayer();
            foreach (var wp in waypointMapLayer.ownWaypoints.Where(wp => wp.Icon == icon)
                .Where(wp => wp.Position.AsBlockPos.InRangeHorizontally(pos.X, pos.Z, radius)))
            {
                if (string.IsNullOrWhiteSpace(partialTitle)) return true;
                if (wp.Title.Contains(partialTitle)) return true;
            }

            return false;
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
            var pos = api.World?.Player?.Entity?.Pos.AsBlockPos.RelativeToSpawn(api.World);
            api.AddWaypointAtPos(pos, icon, colour, title, pinned);
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