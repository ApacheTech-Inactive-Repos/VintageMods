using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VintageMods.Core.Helpers.Enums;
using VintageMods.Core.Helpers.Extensions;
using VintageMods.Core.Helpers.Resources;
using VintageMods.Core.ModSystems.Client;
using VintageMods.WaypointExtensions.Extensions;
using VintageMods.WaypointExtensions.Model;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace VintageMods.WaypointExtensions.Services
{
    /// <summary>
    ///     Business Logic Layer Implementation of the Waypoint Extensions mod.
    /// </summary>
    public sealed class WaypointExtensionsService : ClientSideService
    {
        /// <summary>
        ///     Gets the available waypoint types.
        /// </summary>
        /// <value>The waypoint types.</value>
        private SortedDictionary<string, WaypointInfoModel> WaypointTypes { get; } =
            new SortedDictionary<string, WaypointInfoModel>();

        /// <summary>
        ///     Gets or sets the global configuration.
        /// </summary>
        /// <value>The global configuration.</value>
        private GlobalConfigModel GlobalConfig { get; set; }

        /// <summary>
        ///     Gets a list of all available syntax arguments.
        /// </summary>
        /// <value>The syntax list.</value>
        internal string SyntaxList { get; private set; }

        /// <summary>
        ///     Called when the Start method of the ModSystem is called.
        /// </summary>
        /// <param name="api">The API.</param>
        public override void OnStart(ICoreClientAPI api)
        {
            base.OnStart(api);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    GlobalConfig = Api.LoadOrCreateFile<GlobalConfigModel>(
                        FileType.Config, "wpex-global-config.data");

                    if (Version.Parse(GlobalConfig.Version) < GetCurrentVersion())
                    {
                        Api.UpdateFile<GlobalConfigModel>(
                            FileType.Config, "Waypoint Extensions", "wpex-global-config.data");

                        Api.UpdateFile<WaypointInfoModel>(
                            FileType.Data, "Waypoint Extensions", "wpex-default-waypoints.data");
                    }

                    WaypointTypes.AddRange(
                        Api.PopulateFromFile<WaypointInfoModel>("wpex-default-waypoints.data"));

                    WaypointTypes.AddRange(
                        Api.PopulateFromFile<WaypointInfoModel>("wpex-custom-waypoints.data", false));

                    SyntaxList = string.Join(" | ", WaypointTypes.Keys);
                    api.Logger.Event($"{WaypointTypes.Count} waypoint extensions loaded.");
                }
                catch (Exception e)
                {
                    Api.Logger.Error($"Waypoint Extensions: Error loading syntax for .wp command; {e.Message}");
                }
            });
        }

        /// <summary>
        ///     Handles calls to the .wp chat command.
        /// </summary>
        /// <param name="grpId">The group identifier.</param>
        /// <param name="cmdArgs">The command arguments.</param>
        internal void AddWaypointCommandHandler(int grpId, CmdArgs cmdArgs)
        {
            if (cmdArgs.Length == 0)
            {
                Api.ShowChatMessage(InfoMessage());
                return;
            }

            var arg = cmdArgs.PopWord("");

            var pin = false;
            if (arg == "pin")
            {
                pin = true;
                arg = cmdArgs.PopWord("");
            }

            // Note: For updating the syntax list on the forum.
            if (arg == "copy-info")
            {
                Api.Forms.SetClipboardText(InfoMessage());
                return;
            }

            if (WaypointTypes.ContainsKey(arg))
            {
                var wpInfo = WaypointTypes[arg];
                Api.AddWaypointAtCurrentPos(wpInfo.Icon.ToLower(), wpInfo.Colour.ToLower(),
                    cmdArgs.PopAll(wpInfo.DefaultTitle), pin);
            }
            else
            {
                Api.ShowChatMessage(@"Waypoint Extensions: Invalid Argument.");
            }
        }

        /// <summary>
        ///     Sends the player an information message, if no arguments are given.
        /// </summary>
        private string InfoMessage()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"Waypoint Extensions, by Apache Gaming");
            sb.AppendLine("");
            sb.AppendLine("Quickly, and easily add waypoint markers at your current position.");
            sb.AppendLine("");
            sb.AppendLine($"Syntax: .wp [pin (Optional)] [{SyntaxList}] [title (Optional)]");
            sb.AppendLine("");
            sb.AppendLine("Example: .wp copper");
            sb.AppendLine("Example: .wp pin home Apache");
            sb.AppendLine("Example: .wp trader Trader (Building Supplies)");
            return sb.ToString();
        }

        /// <summary>
        ///     Gets the current version of the mod.
        /// </summary>
        private static Version GetCurrentVersion()
        {
            var data = ResourceManager.ParseResourceAs<GlobalConfigModel>("wpex-global-config.data");
            return Version.Parse(data.Version);
        }
    }
}