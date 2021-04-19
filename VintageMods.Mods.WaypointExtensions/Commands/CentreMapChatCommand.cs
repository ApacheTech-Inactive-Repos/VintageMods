using System;
using System.Linq;
using VintageMods.Core.Common.Reflection;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace VintageMods.Mods.WaypointExtensions.Commands
{
    [ChatCommand("cm", "wpex:cm_Cmd_Description", "wpex:cm_Cmd_Syntax_Message")]
    internal class CentreMapChatCommand : ChatCommandBase<ICoreClientAPI>
    {
        public CentreMapChatCommand(ICoreClientAPI api) : base(api) { }

        public override void OnNoOption(string option, CmdArgs args)
        {
            OnCustomOption(option, args);
        }

        public override void OnCustomOption(string option, CmdArgs args)
        {
            var player = Api.World.Player;
            switch (args.Length)
            {
                // Re-centre on given X, Z coordinates.
                case 2:
                    var x = args.PopInt().GetValueOrDefault(player.Entity.Pos.AsBlockPos.X);
                    var z = args.PopInt().GetValueOrDefault(player.Entity.Pos.AsBlockPos.Z);
                    var pos = new BlockPos(x, 1, z).Add(Api.World.DefaultSpawnPosition.AsBlockPos);
                    Api.ShowChatMessage(Lang.Get("wpex:cm_ReCentre_On_Position", x, z));
                    RecentreMap(pos.ToVec3d());
                    break;

                // Re-centre on a given player.
                case 1:
                    var name = args.PopWord(player.PlayerName);
                    var match = Api.World.AllOnlinePlayers.Where(p =>
                        string.Equals(p.PlayerName, name, StringComparison.InvariantCultureIgnoreCase)).ToList();
                    if (match.Count == 1) player = (IClientPlayer)match.First();
                    Api.ShowChatMessage(Lang.Get("wpex:cm_ReCentre_On_Player", player.PlayerName));
                    RecentreMap(player.Entity.Pos.XYZ);
                    break;

                // Re-centre on self.
                default:
                    Api.ShowChatMessage(Lang.Get("wpex:cm_ReCentre_On_Player", player.PlayerName));
                    RecentreMap(player.Entity.Pos.XYZ);
                    break;
            }
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
                var guiComposer = map.GetField<GuiComposer>("fullDialog");
                var guiElementMap = (GuiElementMap)guiComposer.GetElement("mapElem");

                guiElementMap.CurrentBlockViewBounds.X1 =
                    pos.X - guiElementMap.Bounds.InnerWidth / 2.0 / guiElementMap.ZoomLevel;
                guiElementMap.CurrentBlockViewBounds.Z1 =
                    pos.Z - guiElementMap.Bounds.InnerHeight / 2.0 / guiElementMap.ZoomLevel;
                guiElementMap.CurrentBlockViewBounds.X2 =
                    pos.X + guiElementMap.Bounds.InnerWidth / 2.0 / guiElementMap.ZoomLevel;
                guiElementMap.CurrentBlockViewBounds.Z2 =
                    pos.Z + guiElementMap.Bounds.InnerHeight / 2.0 / guiElementMap.ZoomLevel;

                guiElementMap.EnsureMapFullyLoaded();
            }
            catch (Exception ex)
            {
                Api.Logger.Error(ex.Message);
                Api.Logger.Error(ex.StackTrace);
            }
        }
    }
}
