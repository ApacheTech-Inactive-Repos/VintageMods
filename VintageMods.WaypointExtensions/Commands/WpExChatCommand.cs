using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VintageMods.Core.Client.Extensions;
using VintageMods.Core.Common.Extensions;
using VintageMods.Core.FileIO;
using VintageMods.Core.FileIO.Enum;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Primitives;
using VintageMods.WaypointExtensions.Extensions;
using VintageMods.WaypointExtensions.Model;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable ClassNeverInstantiated.Global

namespace VintageMods.WaypointExtensions.Commands
{
    [ChatCommand("wp", "wpex:wp_Cmd_Description", "wpex:wp_Cmd_Syntax_Message")]
    internal class WpExChatCommand : ChatCommandBase<ICoreClientAPI>
    {
        private FileManager FileSystem { get; }

        private SortedDictionary<string, WaypointInfoModel> WaypointTypes { get; } =
            new SortedDictionary<string, WaypointInfoModel>();

        /// <summary>
        ///     Gets a list of all available syntax arguments.
        /// </summary>
        /// <value>The syntax list.</value>
        private string SyntaxList { get; set; } = "";

        public WpExChatCommand(ICoreClientAPI api) : base(api)
        {
            FileSystem = new FileManager(api, "Waypoint Extensions");

            api.AsClientMain().EnqueueGameLaunchTask(InitialiseComponents, "ats_wpex");
        }

        private void InitialiseComponents()
        {
            try
            {
                var globalConfigFile = FileSystem.RegisterConfigFile<GlobalConfigModel>(
                    "wpex-global-config.data", FileScope.Global);

                var defaultWaypointsFile = FileSystem.RegisterDataFile<WaypointInfoModel>(
                    "wpex-default-waypoints.data", FileScope.Global);

                var customWaypointsFile = FileSystem.RegisterDataFile<WaypointInfoModel>(
                    "wpex-custom-waypoints.data", FileScope.World);

                if (Version.Parse(globalConfigFile.ParseJsonAsObject().Version) < GetCurrentVersion())
                {
                    Api.Logger.Audit("Waypoint Extensions: Updating global default files.");
                    globalConfigFile.Disembed();
                    defaultWaypointsFile.Disembed();
                }

                WaypointTypes.AddRange(defaultWaypointsFile.ParseJsonAsList(), p => p.Syntax);
                WaypointTypes.AddRange(customWaypointsFile.ParseJsonAsList(), p => p.Syntax);
                SyntaxList = string.Join(" | ", WaypointTypes.Keys);

                Api.Logger.Event($"{WaypointTypes.Count} waypoint extensions loaded.");
            }
            catch (Exception e)
            {
                Api.Logger.Error($"Waypoint Extensions: Error loading syntax for .wp command; {e.Message}");
            }
        }

        private static Version GetCurrentVersion()
        {
            var data = ResourceManager.ParseResourceAs<GlobalConfigModel>("wpex-global-config.data");
            return Version.Parse(data.Version);
        }

        public override void OnCustomOption(string option, CmdArgs args)
        {
            var pin = false;
            if (option == "pin")
            {
                pin = true;
                option = args.PopWord("");
            }

            if (WaypointTypes.ContainsKey(option))
            {
                var wpInfo = WaypointTypes[option];
                Api.AddWaypointAtCurrentPos(wpInfo.Icon.ToLower(), wpInfo.Colour.ToLower(),
                    args.PopAll(wpInfo.DefaultTitle), pin);
            }
            else
            {
                Api.ShowChatMessage(Lang.Get("wpex:wp_Cmd_Error_Invalid_Argument"));
            }
        }

        public override string HelpText()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Lang.Get("wpex:mod_Title"));
            sb.AppendLine("");
            sb.AppendLine(Lang.Get("wpex:wp_Cmd_Description"));
            sb.AppendLine("");
            sb.AppendLine(Lang.Get("wpex:wp_Cmd_Syntax_Message_Full", SyntaxList));
            return sb.ToString();
        }

        [ChatOption("DEBUG")]
        public void DebugOptions(string option, CmdArgs args)
        {
            var arg = args.PopWord("");
            switch (arg)
            {
                case "blurb":
                    Api.Forms.SetClipboardText(HelpText());
                    Api.ShowChatMessage("WP Debug: Copied info text to clipboard.");
                    break;
                case "update":
                    UpdateFilesAndFolders();
                    break;
                case "reset":
                    FileSystem.Purge();
                    Api.AsClientMain().EnqueueMainThreadTask(InitialiseComponents, "ats_wpex");
                    break;
                case "purge":
                    FileSystem.Purge();
                    break;
                default:
                    Api.ShowChatMessage("WP Debug: Unknown Command.");
                    break;
            }
        }

        public void UpdateFilesAndFolders()
        {
            var dataRoot = new DirectoryInfo(Path.Combine(GamePaths.DataPath, FileType.Data.ToString(), "Waypoint Extensions"));
            var worldDataRoot = new DirectoryInfo(Path.Combine(dataRoot.FullName, Api.GetSeed()));
            var file = FileManager.RecursiveSearch(worldDataRoot, "wpex-custom-waypoints.data");
            var destPath = Path.Combine(dataRoot.FullName, FileScope.World.ToString(), Api.GetSeed(), "wpex-custom-waypoints.data");

            if (file == null)
            {
                if (File.Exists(destPath))
                {
                    Api.ShowChatMessage("WP Debug: File structure already updated.");
                    return;
                }
                Api.ShowChatMessage("WP Debug: File structure corrupted. Resetting mod to factory settings.");
                FileSystem.Purge();
                Api.AsClientMain().EnqueueMainThreadTask(InitialiseComponents, "ats_wpex");
                return;
            }

            if (File.Exists(destPath))
            {
                File.Delete(destPath);
            }
            File.Move(file.FullName, destPath);

            Api.AsClientMain().EnqueueMainThreadTask(InitialiseComponents, "ats_wpex");
            Api.ShowChatMessage("WP Debug: Custom waypoints file updated.");
        }
    }
}