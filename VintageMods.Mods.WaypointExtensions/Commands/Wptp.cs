using JetBrains.Annotations;
using VintageMods.Core.Extensions;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Exenstions;
using VintageMods.Core.FluentChat.Primitives;
using VintageMods.Core.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace VintageMods.Mods.WaypointExtensions.Commands
{
    [FluentChatCommand("wptp")]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal class Wptp : FluentChatCommandBase<ICoreClientAPI>
    {
        public Wptp(ICoreClientAPI api) : base(api) { }

        public override void OnNoOption(string option, CmdArgs args)
        {
            AddTeleporterWaypoint(false);
        }

        [FluentChatOption("pin")]
        private void OnPinnedOption(string option, CmdArgs args)
        {
            AddTeleporterWaypoint(true);
        }

        private void AddTeleporterWaypoint(bool pinned)
        {
            var found = false;
            var teleporter = Api.World.GetNearestBlockEntity<BlockEntityTeleporter>(Api.World.Player.Entity.Pos.AsBlockPos,
                5f, 1f, _ =>
                {
                    found = true;
                    return true;
                });

            if (!found)
            {
                Api.ShowChatMessage(LangEx.Error("TeleporterNotFound"));
            }
            else
            {
                var sourcePos = teleporter.Pos.RelativeToSpawn(Api.World);
                var tpLocation = teleporter.GetField<TeleporterLocation>("tpLocation");

                // Add Source TP Waypoint.
                if (Api.WaypointExistsAtPos(teleporter.Pos, p => p.Icon == "spiral")) return;
                var title = LangEx.Message("TeleporterWaypoint",
                    tpLocation?.TargetName?.IfNullOrWhitespace("Unknown"));

                Api.AddWaypointAtPos(sourcePos, "spiral", "SpringGreen", title, false);
                Api.Logger.VerboseDebug($"Added Waypoint: {title}");
            }

        }
    }
}