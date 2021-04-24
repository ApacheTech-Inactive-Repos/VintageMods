using System;
using System.Collections.Generic;
using VintageMods.Core.Common.Extensions;
using VintageMods.Core.Common.Reflection;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Exenstions;
using VintageMods.Core.FluentChat.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace VintageMods.Mods.WaypointExtensions.Commands
{
    [FluentChatCommand("wputil")]
    public class WpUtilChatCommand : FluentChatCommandBase<ICoreClientAPI>
    {
        private readonly WorldMapManager _mapManager;

        public WpUtilChatCommand(ICoreClientAPI api) : base(api)
        {
            _mapManager = Api.ModLoader.GetModSystem<WorldMapManager>();
        }

        [FluentChatOption("purge-icon")]
        private void PurgeWaypointsByIcon(string option, CmdArgs args)
        {
            var confirm = LangEx.Phrases("Confirm");

            if (args.Length == 1)
            {
                var icon = args.PopWord();
                Api.ShowChatMessage(icon == confirm
                    ? LangEx.Message("SpecifyIcon")
                    : LangEx.Message("ConfirmationRequest", icon));
            }

            else if (args.Length == 2)
            {
                var icon = args.PopWord();
                if (args.PopWord() != confirm) return;

                try
                {
                    Api.ShowChatMessage(LangEx.Message("PurgingWaypoints", icon, confirm));
                    var i = icon;
                    PurgeWaypoints(p => p.Icon == i);
                }
                catch (Exception ex)
                {
                    Api.Logger.Audit(ex.Message);
                    Api.Logger.Audit(ex.StackTrace);
                }
            }

            else
            {
                Api.ShowChatMessage(LangEx.Message("SpecifyIcon"));
            }
        }

        // For some reason, this won't work as an extension method.
        private void PurgeWaypoints(System.Func<Waypoint, bool> comparer)
        {
            TyronThreadPool.QueueTask(() => {
                var wpLayer = _mapManager.WaypointMapLayer();
                var waypoints = new List<int>();
                for (var i = 0; i < wpLayer.ownWaypoints.Count; i++)
                {
                    if (comparer(wpLayer.ownWaypoints[i]))
                    {
                        waypoints.Add(i);
                    }
                }
                waypoints.Sort((a, b) => b.CompareTo(a));

                foreach (var num in waypoints)
                {
                    Api.Event.EnqueueMainThreadTask(() => Api.SendChatMessage($"/waypoint remove {num}"), "");
                }

                Api.Event.EnqueueMainThreadTask(() =>
                    Api.Event.RegisterCallback(dt =>
                        _mapManager.GetField<IClientNetworkChannel>("clientChannel")
                            .SendPacket(new OnViewChangedPacket()), 500), "");
            });
        }
    }
}