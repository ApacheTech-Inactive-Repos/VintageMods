using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Delegates;
using VintageMods.Core.FluentChat.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
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
            var options = new Dictionary<string, ChatCommandOptionDelegate>();

            foreach (var methodInfo in cmdType.GetMethods())
            {
                var optAttributes = methodInfo.GetCustomAttributes().OfType<ChatOptionAttribute>().ToList();
                if (!optAttributes.Any()) continue;
                foreach (var option in optAttributes)
                {
                    options.Add(option.Name, DelegateEx.CreateDelegate<ChatCommandOptionDelegate>(cmd, methodInfo));
                }
            }

            void CallHandler(int id, CmdArgs cmdArgs)
            {
                var option = cmdArgs.PeekWord();
                if (string.IsNullOrWhiteSpace(option))
                {
                    cmd.OnNoOption(string.Empty, cmdArgs);
                }
                else if (!options.ContainsKey(option))
                {
                    cmd.OnCustomOption(option, cmdArgs);
                }
                else
                {
                    options[option].Invoke(cmdArgs.PopWord(), cmdArgs);
                }
            }

            api.RegisterCommand(cmdAttribute.Name,
                Lang.Get(cmdAttribute.Description),
                Lang.Get(cmdAttribute.SyntaxMessage),
                CallHandler);
        }
    }
}