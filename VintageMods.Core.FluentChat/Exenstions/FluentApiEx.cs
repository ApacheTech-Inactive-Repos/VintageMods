using System.Linq;
using System.Reflection;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Config;
using Vintagestory.Client.NoObf;

// ReSharper disable MemberCanBePrivate.Global

namespace VintageMods.Core.FluentChat.Exenstions
{
    public static class FluentApiEx
    {
        /// <summary>
        ///     Registers the client chat command with the core client app.
        /// </summary>
        /// <typeparam name="TChatCommand">The <see cref="T:System.Type"></see> of chat command to register.</typeparam>
        /// <param name="game">The client app to register the command to.</param>
        public static void RegisterClientChatCommand<TChatCommand>(this ClientMain game)
            where TChatCommand : ChatCommandBase<ICoreClientAPI>
        {
            ((ICoreClientAPI)game.Api).RegisterClientChatCommand<TChatCommand>();
        }

        /// <summary>
        ///     Registers the client chat command with the core client app API.
        /// </summary>
        /// <typeparam name="TChatCommand">The <see cref="T:System.Type"></see> of chat command to register.</typeparam>
        /// <param name="api">The client app API to register the command to.</param>
        public static void RegisterClientChatCommand<TChatCommand>(this ICoreClientAPI api)
            where TChatCommand : ChatCommandBase<ICoreClientAPI>
        {
            var cmdType = typeof(TChatCommand);
            var cmdAttribute = cmdType.GetCustomAttributes().OfType<ChatCommandAttribute>().FirstOrDefault();
            if (cmdAttribute == null || string.IsNullOrEmpty(cmdAttribute.Name)) return;

            var cmd = ActivatorEx.CreateInstance<TChatCommand>(api);
            foreach (var methodInfo in cmdType.GetRuntimeMethods())
            {
                var optAttributes = methodInfo.GetCustomAttributes().OfType<ChatOptionAttribute>().ToList();
                if (!optAttributes.Any()) continue;
                foreach (var option in optAttributes)
                {
                    cmd.Options.Add(option.Name, methodInfo);
                }
            }
            
            api.RegisterCommand(cmdAttribute.Name,
                Lang.Get(cmdAttribute.Description),
                Lang.Get(cmdAttribute.SyntaxMessage),
                cmd.CallHandler);
        }
    }
}