using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VintageMods.Core.ModSystems.Client;
using VintageMods.Core.ModSystems.Extensions;
using VintageMods.Core.ModSystems.IO;
using VintageMods.WaypointExtensions.Extensions;
using VintageMods.WaypointExtensions.Model;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// ReSharper disable ClassNeverInstantiated.Global

namespace VintageMods.WaypointExtensions.Services
{
    /// <summary>
    ///     Business Logic Layer Implementation of the Waypoint Extensions mod.
    /// </summary>
    public sealed class WaypointExtensionsService : ClientSideService
    {
        /// <summary>
        ///     Gets the name of the root folder used by the mod.
        /// </summary>
        /// <value>The name of the root folder used by the mod.</value>
        public override string RootFolder { get; } = "Waypoint Extensions";

        private SortedDictionary<string, WaypointInfoModel> WaypointTypes { get; } =
            new SortedDictionary<string, WaypointInfoModel>();

        private GlobalConfigModel GlobalConfig { get; set; }

        /// <summary>
        ///     Gets a list of all available syntax arguments.
        /// </summary>
        /// <value>The syntax list.</value>
        internal string SyntaxList { get; private set; }

        private void RegisterModFiles()
        {
            ModFiles.RegisterFile("wpex-global-config.data", FileType.Config, FileScope.Global);
            ModFiles.RegisterFile("wpex-default-waypoints.data", FileType.Data, FileScope.Global);
            ModFiles.RegisterFile("wpex-custom-waypoints.data", FileType.Data, FileScope.World);
        }

        /// <summary>
        ///     Called when the Start method of the ModSystem is called.
        /// </summary>
        /// <param name="api">The API.</param>
        public override void OnStart(ICoreClientAPI api)
        {
            base.OnStart(api);
            Init();
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

        private void LoadWaypointTypes()
        {
            WaypointTypes.AddRange(
                ModFiles.AsListOf<WaypointInfoModel>("wpex-default-waypoints.data"));

            WaypointTypes.AddRange(
                ModFiles.AsListOf<WaypointInfoModel>("wpex-custom-waypoints.data"));

            SyntaxList = string.Join(" | ", WaypointTypes.Keys);
            Api.Logger.Event($"{WaypointTypes.Count} waypoint extensions loaded.");
        }

        private void LoadGlobalConfig()
        {
            GlobalConfig = ModFiles.As<GlobalConfigModel>("wpex-global-config.data");

            if (Version.Parse(GlobalConfig.Version) < GetCurrentVersion())
            {
                Api.Logger.Audit("Waypoint Extensions: Updating global default files.");
                ModFiles.SaveFromResource<GlobalConfigModel>("wpex-global-config.data");
                ModFiles.SaveFromResource<GlobalConfigModel>("wpex-default-waypoints.data");
            }
        }

        private static Version GetCurrentVersion()
        {
            var data = ResourceManager.ParseResourceAs<GlobalConfigModel>("wpex-global-config.data");
            return Version.Parse(data.Version);
        }

        private void Init()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    RegisterModFiles();
                    LoadGlobalConfig();
                    LoadWaypointTypes();
                }
                catch (Exception e)
                {
                    Api.Logger.Error($"Waypoint Extensions: Error loading syntax for .wp command; {e.Message}");
                }
            });
        }

        public void ReloadFiles()
        {
            ModFiles.Purge();
            Init();
        }
    }
}