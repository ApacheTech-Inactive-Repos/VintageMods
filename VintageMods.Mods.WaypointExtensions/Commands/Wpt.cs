using System.Collections.Generic;
using System.Linq;
using VintageMods.Core.Client.Extensions;
using VintageMods.Core.Common.Enum;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Exenstions;
using VintageMods.Core.FluentChat.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable ClassNeverInstantiated.Global

namespace VintageMods.Mods.WaypointExtensions.Commands
{
    [FluentChatCommand("wpt")]
    internal class Wpt : FluentChatCommandBase<ICoreClientAPI>
    {
        private readonly Dictionary<string, string> _traderColours = new Dictionary<string, string>
        {
            { TraderType.Artisan, "Aqua" },
            { TraderType.BuildingSupplies, "Red" },
            { TraderType.Clothing, "Green" },
            { TraderType.Commodities, "Gray" },
            { TraderType.Foods, "#C8C080" },
            { TraderType.Furniture, "Orange" },
            { TraderType.Luxuries, "Blue" },
            { TraderType.SurvivalGoods, "Yellow" },
            { TraderType.TreasureHunter, "Purple" },
        };

        public Wpt(ICoreClientAPI api) : base(api) { }

        public override void OnNoOption(string option, CmdArgs args)
        {
            AddTraderWaypoint(false);
        }

        [FluentChatOption("pin")]
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
                Api.ShowChatMessage(LangEx.Error("TraderNotFound"));
            }
            else
            {
                var pos = trader.Pos.AsBlockPos.RelativeToSpawn(Api.World);
                var displayName = trader.GetBehavior<EntityBehaviorNameTag>().DisplayName;

                if (Api.WaypointExistsWithinRadius(trader.Pos.AsBlockPos, 10, 
                    p => p.Icon == "trader" && p.Title.Contains(displayName)))
                {
                    Api.ShowChatMessage(LangEx.Error("WaypointAlreadyExists"));
                    return;
                }

                var colour = TraderIconColour(trader.Code.Path);
                var wpTitle = Lang.Get("tradingwindow-" + trader.Code.Path, displayName);
                Api.AddWaypointAtPos(pos, "trader", colour, wpTitle, pinned);
            }
        }

        private string TraderIconColour(string path)
        {
            return _traderColours.SingleOrDefault(p => 
                path.ToLowerInvariant().EndsWith(p.Key)).Value ?? "White";
        }
    }
}
