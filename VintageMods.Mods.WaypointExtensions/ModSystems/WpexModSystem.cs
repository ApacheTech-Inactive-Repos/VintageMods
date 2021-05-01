using System;
using VintageMods.Core.Client.ModSystems;
using VintageMods.Core.Common.Attributes;
using VintageMods.Core.FileIO.Enum;
using VintageMods.Core.FileIO.Extensions;
using VintageMods.Core.FluentChat.Exenstions;
using VintageMods.Mods.WaypointExtensions.Commands;
using VintageMods.Mods.WaypointExtensions.UI;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global

[assembly: ModDomain("wpex", "Waypoint Extensions")]
namespace VintageMods.Mods.WaypointExtensions.ModSystems
{
    internal class WpexModSystem : ClientSideModSystem
    {
        private IClientNetworkChannel _clientChannel;

        public override void StartClientSide(ICoreClientAPI api)
        {
            api.RegisterFileManager(
                ("wpex-global-config.data", FileScope.Global),
                ("wpex-default-waypoints.data", FileScope.Global),
                ("wpex-custom-waypoints.data", FileScope.World),
                ("wpex-settings.data", FileScope.World)
            );

            api.RegisterClientChatCommand<Wp>();
            api.RegisterClientChatCommand<Wpex>();
            api.RegisterClientChatCommand<Wpt>();

            // Will eventually be shipped to Campaign Cartographer Mod.
            api.RegisterClientChatCommand<Cm>();
            api.RegisterClientChatCommand<Gps>();

            var settingsWindow = new SettingsWindow(api);
            api.Input.RegisterHotKey("wpex-settings", "Waypoint Extensions Settings", GlKeys.F7,
                HotkeyType.GUIOrOtherControls);
            api.Input.SetHotKeyHandler("wpex-settings", a =>
            {
                api.Event.RegisterCallback(d => settingsWindow.Toggle(), 100);
                return true;
            });
        }

        public override void Dispose()
        {
            Api.UnregisterFluentChatCommands();
            base.Dispose();
        }
    }
}
    