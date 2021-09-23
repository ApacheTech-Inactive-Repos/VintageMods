using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using VintageMods.Core.FluentChat.Extensions;
using VintageMods.Core.Reflection;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace VintageMods.Core.FluentChat.Primitives
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithInheritors)]
    public abstract class FluentChatCommandBase<TApi> : IDisposable where TApi : ICoreAPI
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="FluentChatCommandBase{TApi}" /> class.
        /// </summary>
        /// <param name="api">The sided core app api, used to call game mechanics.</param>
        protected FluentChatCommandBase(TApi api)
        {
            Api = api;
            Options = new Dictionary<string, MethodInfo>();
        }

        protected TApi Api { get; }

        internal Dictionary<string, MethodInfo> Options { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal void CallHandler(int groupId, CmdArgs cmdArgs)
        {
            try
            {
                var option = cmdArgs.PeekWord();
                if (string.IsNullOrWhiteSpace(option))
                {
                    OnNoOption(string.Empty, cmdArgs);
                }
                else if (!Options.ContainsKey(option))
                {
                    option = cmdArgs.PopWord();
                    OnCustomOption(option, cmdArgs);
                }
                else
                {
                    option = cmdArgs.PopWord();
                    OnKnownOption(Options[option], option, cmdArgs);
                }
            }
            catch (Exception ex)
            {
                Api.Logger.Audit(ex.Message);
                Api.Logger.Error(ex.Message);
                Api.Logger.Error(ex.StackTrace);
            }
        }

        /// <summary>
        ///     Called when the given option is not found within the FluentOption dictionary.
        /// </summary>
        /// <param name="option">The option passed to the command.</param>
        /// <param name="args">The remaining arguments.</param>
        public virtual void OnCustomOption(string option, CmdArgs args)
        {
            ((ClientMain) Api.World).ShowChatMessage(HelpText());
        }

        /// <summary>
        ///     Called when no arguments are passed to the command.
        ///     By default, this shows the help text for the command. This behaviour can be overridden.
        /// </summary>
        /// <param name="option">The option. This should be an empty string.</param>
        /// <param name="args">The arguments. This should be an empty list.</param>
        public virtual void OnNoOption(string option, CmdArgs args)
        {
            ((ClientMain) Api.World).ShowChatMessage(HelpText());
        }

        /// <summary>
        ///     Called when the given option is found within the FluentOption dictionary.
        /// </summary>
        /// <param name="method">The method name to call.</param>
        /// <param name="option">The option passed to the command.</param>
        /// <param name="args">The remaining arguments.</param>
        public virtual void OnKnownOption(MethodInfo method, string option, CmdArgs args)
        {
            this.CallMethod(method.Name, option, args);
        }

        /// <summary>
        ///     The default help text to display when using the .help command.
        /// </summary>
        /// <returns>By default, returns the description of the command, as set within the language files. This can be overridden.</returns>
        public virtual string HelpText()
        {
            return LangEx.FluentChat(this, "Description");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }
    }
}