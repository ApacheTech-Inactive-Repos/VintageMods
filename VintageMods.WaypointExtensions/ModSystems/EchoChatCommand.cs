using JetBrains.Annotations;
using VintageMods.Core.Fluent.Attributes;
using VintageMods.Core.Fluent.Primitives;
using VintageMods.WaypointExtensions.Services;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// ReSharper disable UnusedParameter.Global

namespace VintageMods.WaypointExtensions.ModSystems
{
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature,
        ImplicitUseTargetFlags.WithMembers)]
    [FluentCommand("wp-debug", Description = "Test functions for the fluent command system.")]
    public sealed class DebugChatCommand : FluentClientCommand
    {
        private WaypointExtensionsService Service { get; }

        public DebugChatCommand(ICoreClientAPI api, WaypointExtensionsService service) : base(api)
        {
            Service = service;
        }

        public override string HelpText()
        {
            return Service.InfoMessage();
        }

        [FluentOption("purge")]
        public void PurgeFilesAndFolders(string option, CmdArgs args)
        {
            Service.ReloadFiles();
        }

        [FluentOption("blurb")]
        public void CopyBlurb(string option, CmdArgs args)
        {
            Api.Forms.SetClipboardText(Service.InfoMessage());
            Api.ShowChatMessage("WP Debug: Copied info text to clipboard.");
        }
    }
}