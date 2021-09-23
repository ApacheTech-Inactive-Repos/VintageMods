using System.Linq;
using System.Reflection;
using VintageMods.Core.Attributes;
using VintageMods.Core.FluentChat.Attributes;
using Vintagestory.API.Config;

namespace VintageMods.Core.FluentChat.Extensions
{
    /// <summary>
    ///     Utility class for enabling i18n. Loads language entries from assets/[category]/[locale].json
    /// </summary>
    public static class LangEx
    {
        private static string Get(string code, string section, object[] args)
        {
            var modDomain = Assembly.GetCallingAssembly().GetCustomAttributes()
                .OfType<ModDomainAttribute>().FirstOrDefault()?.Domain ?? "VintageMods";
            return Lang.Get(code.StartsWith(modDomain) ? code : $"{modDomain}:{section}.{code}", args);
        }

        /// <summary>
        ///     Get the the lang entry for given key, returns the key itself it the entry does not exist.
        /// </summary>
        /// <param name="code">The code that represents the localised string.</param>
        /// <param name="args">Any arguments that are passed into the localised string format mask.</param>
        /// <returns>A localised string, based on the user's language settings within the game.</returns>
        public static string Error(string code, params object[] args)
        {
            return Get(code, "Errors", args);
        }

        /// <summary>
        ///     Get the the lang entry for given key, returns the key itself it the entry does not exist.
        /// </summary>
        /// <param name="code">The code that represents the localised string.</param>
        /// <param name="args">Any arguments that are passed into the localised string format mask.</param>
        /// <returns>A localised string, based on the user's language settings within the game.</returns>
        public static string UI(string code, params object[] args)
        {
            return Get(code, "UI", args);
        }

        /// <summary>
        ///     Get the the lang entry for given key, returns the key itself it the entry does not exist.
        /// </summary>
        /// <param name="code">The code that represents the localised string.</param>
        /// <param name="args">Any arguments that are passed into the localised string format mask.</param>
        /// <returns>A localised string, based on the user's language settings within the game.</returns>
        public static string Message(string code, params object[] args)
        {
            return Get(code, "Messages", args);
        }

        /// <summary>
        ///     Get the the lang entry for given key, returns the key itself it the entry does not exist.
        /// </summary>
        /// <param name="code">The code that represents the localised string.</param>
        /// <param name="args">Any arguments that are passed into the localised string format mask.</param>
        /// <returns>A localised string, based on the user's language settings within the game.</returns>
        public static string Meta(string code, params object[] args)
        {
            return Get(code, "Meta", args);
        }

        /// <summary>
        ///     Get the the lang entry for given key, returns the key itself it the entry does not exist.
        /// </summary>
        /// <param name="code">The code that represents the localised string.</param>
        /// <param name="args">Any arguments that are passed into the localised string format mask.</param>
        /// <returns>A localised string, based on the user's language settings within the game.</returns>
        public static string Phrases(string code, params object[] args)
        {
            return Get(code, "Phrases", args);
        }

        /// <summary>
        ///     Get the the lang entry for given key, returns the key itself it the entry does not exist.
        /// </summary>
        /// <param name="command">The chat command this message pertains to.</param>
        /// <param name="code">The code that represents the localised string.</param>
        /// <param name="args">Any arguments that are passed into the localised string format mask.</param>
        /// <returns>A localised string, based on the user's language settings within the game.</returns>
        public static string FluentChat(object command, string code, params object[] args)
        {
            var cmdAttribute = command.GetType().GetCustomAttributes().OfType<FluentChatCommandAttribute>()
                .FirstOrDefault();
            return Get(code, $"ChatCommands.{cmdAttribute?.Name ?? "NULL"}", args);
        }
    }
}