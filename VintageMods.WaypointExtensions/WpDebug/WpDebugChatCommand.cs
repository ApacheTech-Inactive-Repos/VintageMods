using System.IO;
using JetBrains.Annotations;
using VintageMods.Core.Extensions;
using VintageMods.Core.Fluent.Attributes;
using VintageMods.Core.Fluent.Primitives;
using VintageMods.Core.IO;
using VintageMods.WaypointExtensions.Services;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

// ReSharper disable UnusedParameter.Global

namespace VintageMods.WaypointExtensions.WpDebug
{
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature,
        ImplicitUseTargetFlags.WithMembers)]
    [FluentCommand("wp-debug", Description = "Test functions for the fluent command system.")]
    public sealed class WpDebugChatCommand : FluentClientCommand
    {
        private WaypointExtensionsService Service { get; }

        public WpDebugChatCommand(ICoreClientAPI api, WaypointExtensionsService service) : base(api)
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
            Service.Purge();
        }

        [FluentOption("reset")]
        public void ResetMod(string option, CmdArgs args)
        {
            Service.Purge();
            Service.ReloadFiles();
        }

        [FluentOption("update")]
        public void UpdateFilesAndFolders(string option, CmdArgs args)
        {
            var dataRoot = new DirectoryInfo(Path.Combine(GamePaths.DataPath, FileType.Data, Service.RootFolder));
            var worldDataRoot = new DirectoryInfo(Path.Combine(dataRoot.FullName, Api.GetSeed()));
            var file = FileManager.RecursiveSearch(worldDataRoot, "wpex-custom-waypoints.data");
            var destPath = Path.Combine(dataRoot.FullName, FileScope.World, Api.GetSeed(), "wpex-custom-waypoints.data");

            if (file == null)
            {
                if (File.Exists(destPath))
                {
                    Api.ShowChatMessage("WP Debug: File structure already updated.");
                    return;
                }
                Api.ShowChatMessage("WP Debug: File structure corrupted. Resetting mod to factory settings.");
                ResetMod(option, args);
                return;
            }

            if (File.Exists(destPath))
            {
                File.Delete(destPath);
            }
            File.Move(file.FullName, destPath);

            Service.ReloadFiles();
            Api.ShowChatMessage("WP Debug: Custom waypoints file updated.");
        }

        [FluentOption("blurb")]
        public void CopyBlurb(string option, CmdArgs args)
        {
            Api.Forms.SetClipboardText(Service.InfoMessage());
            Api.ShowChatMessage("WP Debug: Copied info text to clipboard.");
        }
    }
}