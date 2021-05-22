using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VintageMods.Core.Attributes;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Config;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace VintageMods.Core.FluentChat.Extensions
{
    /// <summary>
    ///     Utility class that adds extension methods for handling file IO within VintageMods mods.
    /// </summary>
    public static class FluentApiEx
    {
        private static Dictionary<string, FluentChatCommandBase<ICoreClientAPI>> ClientCommands { get; } = new();

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
        ///     Retrieves a client chat command instance that has been registered with the API.
        /// </summary>
        /// <typeparam name="TChatCommand">The <see cref="T:System.Type"></see> of chat command to retrieve.</typeparam>
        /// <param name="api">The client app API to register the command to.</param>
        /// <param name="command">The name of the command to retrieve.</param>
        public static TChatCommand GetFluentChatCommand<TChatCommand>(this ICoreClientAPI api, string command) where TChatCommand : FluentChatCommandBase<ICoreClientAPI>
        {
            ClientCommands.TryGetValue(command, out var cmd);
            return cmd as TChatCommand;
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

            var cmdAttribute = cmdType.GetCustomAttributes()
                .OfType<FluentChatCommandAttribute>().FirstOrDefault();

            if (string.IsNullOrEmpty(cmdAttribute?.Name)) return;

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

            ClientCommands.Add(cmdAttribute.Name, cmd);
        }

        /// <summary>
        ///     Un-registers any fluent chat commands that have been registered by the mod.
        /// </summary>
        /// <param name="api">The core game API.</param>
        public static void UnregisterFluentChatCommands(this ICoreClientAPI api)
        {
            foreach (var cmd in ClientCommands.Values) cmd.Dispose();
            ClientCommands.Clear();
        }
    }
}