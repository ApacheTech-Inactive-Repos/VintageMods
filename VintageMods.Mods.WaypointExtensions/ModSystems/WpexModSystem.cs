using VintageMods.Core.Attributes;
using VintageMods.Core.FluentChat.Extensions;
using VintageMods.Core.IO.Enum;
using VintageMods.Core.IO.Extensions;
using VintageMods.Core.ModSystems;
using VintageMods.Mods.WaypointExtensions.Commands;
using VintageMods.Mods.WaypointExtensions.Model;
using VintageMods.Mods.WaypointExtensions.UI;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

[assembly: ModDomain("wpex", "Waypoint Extensions")]
namespace VintageMods.Mods.WaypointExtensions.ModSystems
{
    internal class WpexModSystem : ClientSideModSystem
    {
        internal WorldSettings Settings { get; private set; }

        public override void StartClientSide(ICoreClientAPI api)
        {
            Files.RegisterFile("wpex-global-config.data", FileScope.Global);
            Files.RegisterFile("wpex-default-waypoints.data", FileScope.Global);
            Files.RegisterFile("wpex-custom-waypoints.data", FileScope.World);
            Files.RegisterFile("wpex-settings.data", FileScope.World);

            Settings = Api.GetModFile("wpex-settings.data").ParseJsonAsObject<WorldSettings>();
            RegisterChatCommands(api);

            var settingsWindow = new SettingsWindow(api);
            api.Input.RegisterHotKey("wpex-settings", "Waypoint Extensions Settings", GlKeys.F7, HotkeyType.GUIOrOtherControls);
            api.Input.SetHotKeyHandler("wpex-settings", _ =>
            {
                api.Event.RegisterCallback(_ =>
                {
                    settingsWindow.ComposeWindow();
                    settingsWindow.Toggle();
                }, 100);
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
    