using System;
using System.Collections.Generic;
using ApacheTech.WaypointExtensions.Mod.ModSystems;
using ApacheTech.WaypointExtensions.Mod.Patches;
using VintageMods.Core.Extensions;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Extensions;
using VintageMods.Core.FluentChat.Primitives;
using VintageMods.Core.IO.Extensions;
using VintageMods.Core.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace ApacheTech.WaypointExtensions.Mod.Commands
{
    [FluentChatCommand("wpex")]
    public class Wpex : FluentChatCommandBase<ICoreClientAPI>
    {
        private readonly WorldMapManager _mapManager;
        
        private WpexModSystem Mod { get; }

        public Wpex(ICoreClientAPI api) : base(api)
        {
            _mapManager = Api.ModLoader.GetModSystem<WorldMapManager>();
            Mod = api.ModLoader.GetModSystem<WpexModSystem>();
            WpexPatches.Api = api;
        }

        [FluentChatOption("debug")]
        private void DebugOptions(string option, CmdArgs args)
        {
            var arg = args.PopWord("");
            switch (arg)
            {
                case "blurb":
                    var cmd = Api.GetFluentChatCommand<Wp>("wp");
                    Api.Forms.SetClipboardText(cmd.HelpText());
                    Api.ShowChatMessage("WP Debug: Copied info text to clipboard.");
                    break;
                default:
                    Api.ShowChatMessage("WP Debug: Unknown Command.");
                    break;
            }
        }

        [FluentChatOption("auto-tl")]
        private void AutomaticTranslocators(string option, CmdArgs args)
        {
            Mod.Settings.AutoTranslocatorWaypoints = !Mod.Settings.AutoTranslocatorWaypoints;
            Api.GetModFile("wpex-settings.data").SaveAsJson(Mod.Settings);
            Api.ShowChatMessage(LangEx.Message("AutoTranslocator", Mod.Settings.AutoTranslocatorWaypoints));
        }

        [FluentChatOption("auto-tr")]
        private void AutomaticTraders(string option, CmdArgs args)
        {
            Mod.Settings.AutoTraderWaypoints = !Mod.Settings.AutoTraderWaypoints;
            Api.GetModFile("wpex-settings.data").SaveAsJson(Mod.Settings);
            Api.ShowChatMessage(LangEx.Message("AutoTrader", Mod.Settings.AutoTraderWaypoints));
        }

        private bool OnInteractWithTrader()
        {
            Api.SendChatMessage(".wpt");
            return true;
        }


        [FluentChatOption("purge-nearby")]
        private void PurgeWaypointsNearby(string option, CmdArgs args)
        {
            var radius = args.PopFloat().GetValueOrDefault(10f);
            PurgeWaypoints(p => 
                Api.World.Player.Entity.Pos.InHorizontalRangeOf(
                    p.Position.AsBlockPos.X, p.Position.AsBlockPos.Z, radius));
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