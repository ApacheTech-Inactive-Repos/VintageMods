using System.Linq;
using System.Reflection;
using VintageMods.Core.Common.Attributes;
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
            where TChatCommand : FluentChatCommandBase<ICoreClientAPI>
        {
            ((ICoreClientAPI)game.Api).RegisterClientChatCommand<TChatCommand>();
        }

        /// <summary>
        ///     Registers the client chat command with the core client app API.
        /// </summary>
        /// <typeparam name="TChatCommand">The <see cref="T:System.Type"></see> of chat command to register.</typeparam>
        /// <param name="api">The client app API to register the command to.</param>
        public static void RegisterClientChatCommand<TChatCommand>(this ICoreClientAPI api)
            where TChatCommand : FluentChatCommandBase<ICoreClientAPI>
        {
            var cmdType = typeof(TChatCommand);

            var modDomain = cmdType.Assembly.GetCustomAttributes()
                .OfType<ModDomainAttribute>().FirstOrDefault()?.Domain ?? "VintageMods";

            var cmdAttribute = cmdType.GetCustomAttributes().OfType<FluentChatCommandAttribute>().FirstOrDefault();

            if (cmdAttribute == null || string.IsNullOrEmpty(cmdAttribute.Name)) return;

            var cmd = ActivatorEx.CreateInstance<TChatCommand>(api);
            foreach (var methodInfo in cmdType.GetRuntimeMethods())
            {
                var optAttributes = methodInfo.GetCustomAttributes().OfType<FluentChatOptionAttribute>().ToList();
                if (!optAttributes.Any()) continue;
                foreach (var option in optAttributes)
                {
                    cmd.Options.Add(option.Name, methodInfo);
                }
            }
            
            var description = cmdAttribute.Description ??= $"{modDomain}:ChatCommands.{cmdAttribute.Name}.Description";
            var syntaxMessage = cmdAttribute.SyntaxMessage ??= $"{modDomain}:ChatCommands.{cmdAttribute.Name}.SyntaxMessage";

            api.RegisterCommand(cmdAttribute.Name,
                Lang.Get(description),
                Lang.Get(syntaxMessage),
                cmd.CallHandler);
        }
    }
}