using VintageMods.Core.FluentChat.Exenstions;
using VintageMods.WaypointExtensions.Commands;
using Vintagestory.Client.NoObf;

namespace VintageMods.WaypointExtensions.ClientSystems
{
    internal class WpexClientSystem : ClientSystem
    {
        public WpexClientSystem(ClientMain game) : base(game)
        {
            game.RegisterClientChatCommand<WpExChatCommand>();
            game.RegisterClientChatCommand<WpTraderChatCommand>();
            game.RegisterClientChatCommand<CentreMapChatCommand>();
        }

        public override EnumClientSystemType GetSystemType()
        {
            return EnumClientSystemType.Misc;
        }

        public override string Name { get; } = "ats_wpex";
    }
}
