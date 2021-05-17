using System.Linq;
using System.Reflection;
using VintageMods.Core.Common.Attributes;
using VintageMods.Core.FluentChat.Attributes;
using Vintagestory.API.Config;

namespace VintageMods.Core.FluentChat.Exenstions
{
    public static class LangEx
    {
        private static string Get(string code, string section, object[] args)
        {
            var modDomain = Assembly.GetCallingAssembly().GetCustomAttributes()
                .OfType<ModDomainAttribute>().FirstOrDefault()?.Domain ?? "VintageMods";
            return Lang.Get(code.StartsWith(modDomain) ? code : $"{modDomain}:{section}.{code}", args);
        }

        public static string Error(string code, params object[] args)
        {
            return Get(code, "Errors", args);
        }

        public static string UI(string code, params object[] args)
        {
            return Get(code, "UI", args);
        }


        public static string Message(string code, params object[] args)
        {
            return Get(code, "Messages", args);
        }

        public static string Meta(string code, params object[] args)
        {
            return Get(code, "Meta", args);
        }

        public static string Phrases(string code, params object[] args)
        {
            return Get(code, "Phrases", args);
        }

        public static string FluentChat(object command, string code, params object[] args)
        {
            var cmdAttribute = command.GetType().GetCustomAttributes().OfType<FluentChatCommandAttribute>().FirstOrDefault();
            return Get(code, $"ChatCommands.{cmdAttribute?.Name ?? "NULL"}", args);
        }
    }
}