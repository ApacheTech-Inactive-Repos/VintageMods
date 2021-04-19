using System.Runtime.CompilerServices;
using VintageMods.Core.Client.ModSystems;
using VintageMods.Core.FileIO.Enum;
using VintageMods.Core.FileIO.Extensions;
using VintageMods.Core.FluentChat.Exenstions;
using VintageMods.Mods.WaypointExtensions.Commands;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

namespace VintageMods.Mods.WaypointExtensions.ModSystems
{
    internal class WpexModSystem : ClientSideModSystem
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override void StartClientSide(ICoreClientAPI api)
        {
            api.RegisterFileManager("Waypoint Extensions",
                ("wpex-global-config.data", FileType.Config, FileScope.Global),
                ("wpex-default-waypoints.data", FileType.Data, FileScope.Global),
                ("wpex-custom-waypoints.data", FileType.Data, FileScope.World));

            api.RegisterClientChatCommand<WpExChatCommand>();
            api.RegisterClientChatCommand<WpUtilChatCommand>();
            api.RegisterClientChatCommand<WpTraderChatCommand>();

            // Will eventually be shipped to Campaign Cartographer Mod.
            api.RegisterClientChatCommand<CentreMapChatCommand>();

        }
    }
}
