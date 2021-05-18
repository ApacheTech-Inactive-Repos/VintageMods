using System;
using System.Linq;
using VintageMods.Core.Extensions;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Exenstions;
using VintageMods.Core.FluentChat.Primitives;
using VintageMods.Core.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace VintageMods.Mods.WaypointExtensions.Commands
{
    [FluentChatCommand("cm")]
    internal class Cm : FluentChatCommandBase<ICoreClientAPI>
    {
        public Cm(ICoreClientAPI api) : base(api) { }

        [FluentChatOption("self")]
        public override void OnNoOption(string option, CmdArgs args)
        {
            var player = Api.World.Player;

            var displayPos = player.Entity.Pos.AsBlockPos.RelativeToSpawn(Api.World);
            Api.ShowChatMessage(LangEx.Message("RecentreOnPlayer", player.PlayerName, displayPos.X, displayPos.Z));
            RecentreMap(player.Entity.Pos.XYZ);
        }

        public override void OnCustomOption(string option, CmdArgs args)
        {
            var player = Api.World.Player;

            var displayPos = player.Entity.Pos.AsBlockPos.RelativeToSpawn(Api.World);
            Api.ShowChatMessage(LangEx.Message("RecentreOnPlayer", player.PlayerName, displayPos.X, displayPos.Z));
            RecentreMap(player.Entity.Pos.XYZ);
            
        }

        [FluentChatOption("plr")]
        [FluentChatOption("player")]
        public void RecentreMapOnPlayer(string option, CmdArgs args)
        {
            var player = Api.World.Player;
            var name = args.PopWord(player.PlayerName);
            var playerList = Api.World.AllOnlinePlayers.Where(p => 
                p.PlayerName.ToLowerInvariant().StartsWith(name.ToLowerInvariant())).ToList();
            if (playerList.Any()) player = (IClientPlayer)playerList.First();

            var displayPos = player.Entity.Pos.AsBlockPos.RelativeToSpawn(Api.World);
            Api.ShowChatMessage(LangEx.Message("RecentreOnPlayer", player.PlayerName, displayPos.X, displayPos.Z));
            RecentreMap(player.Entity.Pos.XYZ);
        }

        [FluentChatOption("pos")]
        [FluentChatOption("position")]
        public void RecentreMapOnPosition(string option, CmdArgs args)
        {
            var playerPos = Api.World.Player.Entity.Pos.AsBlockPos;
            var x = args.PopInt().GetValueOrDefault(playerPos.X);
            var z = args.PopInt().GetValueOrDefault(playerPos.Z);

            var pos = new BlockPos(x, 1, z).Add(Api.World.DefaultSpawnPosition.AsBlockPos);
            Api.ShowChatMessage(LangEx.Message("RecentreOnPosition", x, z));
            RecentreMap(pos.ToVec3d());
        }

        [FluentChatOption("spawn")]
        public void RecentreMapOnWorldSpawn(string option, CmdArgs args)
        {
            var pos = Api.World.DefaultSpawnPosition.AsBlockPos;
            var displayPos = pos.RelativeToSpawn(Api.World);
            Api.ShowChatMessage(LangEx.Message("RecentreOnPosition", displayPos.X, displayPos.Z));
            RecentreMap(pos.ToVec3d());
        }

        /// <summary>
        ///     Re-centres the map on a specific position.
        /// </summary>
        /// <param name="pos">The position to re-centre the map on.</param>
        private void RecentreMap(Vec3d pos)
        {
            try
            {
                var map = Api.ModLoader.GetModSystem<WorldMapManager>().worldMapDlg;
                UpdateMapGui(map.GetField<GuiComposer>("fullDialog"), pos);
                UpdateMapGui(map.GetField<GuiComposer>("hudDialog"), pos);
            }
            catch (Exception ex)
            {
                Api.Logger.Error(ex.Message);
                Api.Logger.Error(ex.StackTrace);
            }
        }

        private static void UpdateMapGui(GuiComposer composer, Vec3d pos)
        {
            var map = (GuiElementMap)composer.GetElement("mapElem");
            map.CurrentBlockViewBounds.X1 = pos.X - map.Bounds.InnerWidth / 2.0 / map.ZoomLevel;
            map.CurrentBlockViewBounds.Z1 = pos.Z - map.Bounds.InnerHeight / 2.0 / map.ZoomLevel;
            map.CurrentBlockViewBounds.X2 = pos.X + map.Bounds.InnerWidth / 2.0 / map.ZoomLevel;
            map.CurrentBlockViewBounds.Z2 = pos.Z + map.Bounds.InnerHeight / 2.0 / map.ZoomLevel;
            map.EnsureMapFullyLoaded();
        }
    }
}
