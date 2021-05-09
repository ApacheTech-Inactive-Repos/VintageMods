using VintageMods.Core.Client.Extensions;
using VintageMods.Core.Common.Reflection;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Exenstions;
using VintageMods.Core.FluentChat.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local

namespace VintageMods.Mods.WaypointExtensions.Commands
{
    [FluentChatCommand("wptl")]
    internal class Wptl : FluentChatCommandBase<ICoreClientAPI>
    {
        public Wptl(ICoreClientAPI api) : base(api) { }

        public override void OnNoOption(string option, CmdArgs args)
        {
            AddTranslocatorWaypoint(false);
        }

        [FluentChatOption("pin")]
        private void OnPinnedOption(string option, CmdArgs args)
        {
            AddTranslocatorWaypoint(true);
        }

        private void AddTranslocatorWaypoint(bool pinned)
        {
            var found = false;
            var translocator = Api.World.GetNearestBlockEntity<BlockEntityStaticTranslocator>(Api.World.Player.Entity.Pos.AsBlockPos,
                5f, 1f, p =>
                {
                    if (!p.FullyRepaired || !p.Activated || !p.GetField<bool>("canTeleport")) return false;
                    found = true;
                    return true;
                });

            if (!found)
            {
                Api.ShowChatMessage(LangEx.Error("TranslocatorNotFound"));
            }
            else
            {
                var sourcePos = translocator.Pos.RelativeToSpawn(Api.World);
                var destPos = translocator.TargetLocation.RelativeToSpawn(Api.World);

                // Add Source TL Waypoint.
                if (!Api.WaypointExistsAtPos(translocator.Pos, p => p.Icon == "spiral"))
                {
                    Api.AddWaypointAtPos(sourcePos, "spiral", "Fuchsia",
                        LangEx.Message("TranslocatorWaypoint", destPos.X, destPos.Y, destPos.Z), false);
                    Api.Logger.VerboseDebug($"Added Waypoint: Translocator to ({destPos.X}, {destPos.Y}, {destPos.Z})");
                }

                // Add Destination TL Waypoint.
                if (!Api.WaypointExistsAtPos(translocator.TargetLocation, p => p.Icon == "spiral"))
                {
                    Api.AddWaypointAtPos(destPos, "spiral", "Fuchsia",
                        LangEx.Message("TranslocatorWaypoint", sourcePos.X, sourcePos.Y, sourcePos.Z), false);
                    Api.Logger.VerboseDebug($"Added Waypoint: Translocator to ({sourcePos.X}, {sourcePos.Y}, {sourcePos.Z})");
                }
            }
        }
    }
}
