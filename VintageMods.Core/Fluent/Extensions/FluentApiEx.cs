using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VintageMods.Core.Contracts;
using VintageMods.Core.Fluent.Attributes;
using VintageMods.Core.Fluent.Primitives;
using Vintagestory.API.Client;

namespace VintageMods.Core.Fluent.Extensions
{
    public static class ObjectEx
    {
        public static object[] PrependToParamsArray(this object me, params object[] args)
        {
            return new[] { me }.Concat(args).ToArray();
        }
    }

    public static class FluentApiEx
    {
        public static bool RegisterFluentCommand<TFluentCommand, TService>(this ICoreClientAPI api, TService service = null) 
            where TFluentCommand : FluentClientCommand 
            where TService : class, IClientSideService
        {
            var cmdType = typeof(TFluentCommand);
            var cmdAttribute = cmdType.GetCustomAttributes().OfType<FluentCommandAttribute>().FirstOrDefault();

            if (cmdAttribute == null || string.IsNullOrEmpty(cmdAttribute.Name)) return false;
            
            api.Logger.Audit("Registering Fluent Command: " + cmdAttribute.Name);
            
            var cmd = ActivatorEx.CreateInstance<TFluentCommand>(api, service);
            var options = new Dictionary<string, FluentOptionDelegate>();

            foreach (var methodInfo in cmdType.GetMethods())
            {
                var optAttributes = methodInfo.GetCustomAttributes().OfType<FluentOptionAttribute>().ToList();
                if (!optAttributes.Any()) continue;
                foreach (var option in optAttributes)
                {
                    api.Logger.Audit("Found Fluent Option: " + cmdAttribute.Name);
                    options.Add(option.Name, DelegateEx.CreateDelegate<FluentOptionDelegate>(cmd, methodInfo));
                }
            }

            api.RegisterCommand(cmdAttribute.Name, cmdAttribute.Description, cmdAttribute.SyntaxMessage, (id, cmdArgs) =>
            {
                var option = cmdArgs.PopWord();
                if (string.IsNullOrWhiteSpace(option))
                {
                    cmd.OnDefaultOption();
                }
                else if (!options.ContainsKey(option))
                {
                    cmd.OnUnrecognisedOption();
                }
                else
                {
                    options[option].Invoke(option, cmdArgs);
                }
            });

            return true;
        }
    }
}