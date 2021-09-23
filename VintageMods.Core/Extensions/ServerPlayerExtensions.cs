using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace VintageMods.Core.Extensions
{
    public static class ServerPlayerExtensions
    {
        public static void SendMessage(this IServerPlayer player, string message)
        {
            player.SendMessage(GlobalConstants.CurrentChatGroup, message, EnumChatType.Notification);
        }
    }
}