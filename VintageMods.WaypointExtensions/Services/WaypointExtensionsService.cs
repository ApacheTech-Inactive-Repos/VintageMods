using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using VintageMods.Core.Extensions;
using VintageMods.Core.IO;
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
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature, 
        ImplicitUseTargetFlags.WithMembers)]
    public sealed class WaypointExtensionsService : ClientSideService
    {
        /// <summary>
        ///     Gets the name of the root folder used by the mod.
        /// </summary>
        /// <value>The name of the root folder used by the mod.</value>
        public override string RootFolder { get; } = "Waypoint Extensions";

        private SortedDictionary<string, WaypointInfoModel> WaypointTypes { get; } =
            new SortedDictionary<string, WaypointInfoModel>();

        /// <summary>
        ///     Gets a list of all available syntax arguments.
        /// </summary>
        /// <value>The syntax list.</value>
        private string SyntaxList { get; set; }
        
        /// <summary>
        ///     Called when the Start method of the ModSystem is called.
        /// </summary>
        /// <param name="api">The API.</param>
        public override void OnStart(ICoreClientAPI api)
        {
            base.OnStart(api);
            IniialiseModSystem();
        }

        /// <summary>
        ///     Handles calls to the .wp chat command.
        /// </summary>
        /// <param name="grpId">The group identifier.</param>
        /// <param name="cmdArgs">The command arguments.</param>
        internal void OnWpCommand(int grpId, CmdArgs cmdArgs)
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

        internal string InfoMessage()
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
        
        private static Version GetCurrentVersion()
        {
            var data = ResourceManager.ParseResourceAs<GlobalConfigModel>("wpex-global-config.data");
            return Version.Parse(data.Version);
        }

        private void IniialiseModSystem()
        {
            Task.Factory.StartNew(() =>
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
                    Api.Logger.StoryEvent($"{WaypointTypes.Count} waypoint extensions loaded.");
                }
                catch (Exception e)
                {
                    Api.Logger.Error($"Waypoint Extensions: Error loading syntax for .wp command; {e.Message}");
                }
            });
        }

        public void ReloadFiles()
        {
            IniialiseModSystem();
        }

        public void Purge()
        {
            FileSystem.Purge();
            IniialiseModSystem();
        }
    }
}