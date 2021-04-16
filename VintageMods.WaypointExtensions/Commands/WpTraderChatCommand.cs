using VintageMods.Core.Client.Extensions;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable ClassNeverInstantiated.Global

namespace VintageMods.WaypointExtensions.Commands
{
    [ChatCommand("wpt", "wpex:wpt_Cmd_Description", "wpex:wpt_Cmd_Syntax_Message")]
    internal class WpTraderChatCommand : ChatCommandBase<ICoreClientAPI>
    {
        public WpTraderChatCommand(ICoreClientAPI api) : base(api) { }

        public override void OnNoOption(string option, CmdArgs args)
        {
            AddTraderWaypoint(false);
        }

        [ChatOption("pin", "wpex:wpt_CmdOpt_Pin_Desc")]
        private void OnPinnedOption(string option, CmdArgs args)
        {
            AddTraderWaypoint(true);
        }

        private void AddTraderWaypoint(bool pinned)
        {
            var found = false;

            var trader = Api.World.GetNearestEntity(Api.World.Player.Entity.Pos.XYZ, 10f, 10f, p =>
            {
                if (!p.Code.Path.StartsWith("humanoid-trader-") || !p.Alive) return false;
                found = true;
                return true;
            });

            if (!found)
            {
                Api.ShowChatMessage(Lang.Get("wpex:wpt_Error_Trader_Not_Found"));
            }
            else
            {
                var pos = trader.Pos.AsBlockPos.RelativeToSpawn(Api);
                var displayName = trader.GetBehavior<EntityBehaviorNameTag>().DisplayName;

                if (Api.WaypointExistsWithinRadius(trader.Pos.AsBlockPos, 10, "trader", displayName))
                {
                    Api.ShowChatMessage(Lang.Get("wpex:wpt_Error_Waypoint_Already_Exists"));
                    return;
                }

                var colour = TraderIconColour(trader.Code.Path);
                var wpTitle = Lang.Get("tradingwindow-" + trader.Code.Path, displayName);
                Api.AddWaypointAtPos(pos, "trader", colour, wpTitle, pinned);
            }
        }

        private static string TraderIconColour(string path)
        {
            if (path.EndsWith("artisan"))
            {
                return "Aqua";
            }
            if (path.EndsWith("buildmaterials"))
            {
                return "Red";
            }
            if (path.EndsWith("clothing"))
            {
                return "Green";
            }
            if (path.EndsWith("commodities"))
            {
                return "Gray";
            }
            if (path.EndsWith("foods"))
            {
                return "#C8C080";
            }
            if (path.EndsWith("furniture"))
            {
                return "Orange";
            }
            if (path.EndsWith("luxuries"))
            {
                return "Blue";
            }
            if (path.EndsWith("survivalgoods"))
            {
                return "Yellow";
            }
            if (path.EndsWith("treasurehunter"))
            {
                return "Purple";
            }

            return "White";
        }

    }
}
