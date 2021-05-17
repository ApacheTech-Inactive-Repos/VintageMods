using VintageMods.Core.Client.ModSystems;
using VintageMods.Core.Common.Attributes;
using VintageMods.Core.Common.Extensions;
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
        public override void StartClientSide(ICoreClientAPI api)
        {
            api.RegisterFileManager(
                ("wpex-global-config.data", FileScope.Global),
                ("wpex-default-waypoints.data", FileScope.Global),
                ("wpex-custom-waypoints.data", FileScope.World),
                ("wpex-settings.data", FileScope.World)
            );
            RegisterChatCommands(api);

            var settingsWindow = new SettingsWindow(api);
            api.Input.RegisterHotKey("wpex-settings", "Waypoint Extensions Settings", GlKeys.F7, HotkeyType.GUIOrOtherControls);
            api.Input.SetHotKeyHandler("wpex-settings", a =>
            {
                api.Event.RegisterCallback(d => settingsWindow.Toggle(), 100);
                return true;
            });
        }

        private static void RegisterChatCommands(ICoreClientAPI api)
        {
            api.RegisterClientChatCommand<Wp>();
            api.RegisterClientChatCommand<Wpex>();
            api.RegisterClientChatCommand<Wpt>();
            api.RegisterClientChatCommand<Wptl>();
            api.RegisterClientChatCommand<Wptp>();

            // Will eventually be shipped to Campaign Cartographer Mod.
            api.RegisterClientChatCommand<Cm>();
            api.RegisterClientChatCommand<Gps>();
        }

        public override void Dispose()
        {               
            Api.UnregisterFluentChatCommands();
            base.Dispose();
        }
    }
}
    