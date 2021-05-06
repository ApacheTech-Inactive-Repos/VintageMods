using System;
using System.Collections.Generic;
using System.Text;
using VintageMods.Core.Client.Extensions;
using VintageMods.Core.Common.Extensions;
using VintageMods.Core.FileIO;
using VintageMods.Core.FileIO.Extensions;
using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Exenstions;
using VintageMods.Core.FluentChat.Primitives;
using VintageMods.Mods.WaypointExtensions.Extensions;
using VintageMods.Mods.WaypointExtensions.Model;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable ClassNeverInstantiated.Global

namespace VintageMods.Mods.WaypointExtensions.Commands
{
    [FluentChatCommand("wp")]
    internal class Wp : FluentChatCommandBase<ICoreClientAPI>
    {
        private SortedDictionary<string, WaypointInfoModel> WaypointTypes { get; } = new();

        private string SyntaxList { get; set; } = "";

        public Wp(ICoreClientAPI api) : base(api)
        {
            InitialiseComponents();
        }

        private void InitialiseComponents()
        {
            try
            {
                var globalConfigFile = Api.GetModFile("wpex-global-config.data");
                var defaultWaypointsFile = Api.GetModFile("wpex-default-waypoints.data");
                var customWaypointsFile = Api.GetModFile("wpex-custom-waypoints.data");
                var globalConfig = globalConfigFile.ParseJsonAsObject<GlobalConfigModel>();

                if (Version.Parse(globalConfig.Version) < CurrentVersion())
                {
                    Api.Logger.VerboseDebug("Waypoint Extensions: Updating global default files.");
                    defaultWaypointsFile.DisembedFrom(GetType().Assembly);
                    globalConfigFile.DisembedFrom(GetType().Assembly);
                }

                WaypointTypes.AddRange(defaultWaypointsFile.ParseJsonAsList<WaypointInfoModel>(), p => p.Syntax);
                WaypointTypes.AddRange(customWaypointsFile.ParseJsonAsList<WaypointInfoModel>(), p => p.Syntax);
                SyntaxList = string.Join(" | ", WaypointTypes.Keys);

                Api.World.Logger.Event($"{WaypointTypes.Count} waypoint extensions loaded.");
            }
            catch (Exception e)
            {
                Api.Logger.Error($"Waypoint Extensions: Error loading syntax for .wp command; {e.Message}");
                Api.Logger.Error(e.StackTrace);
            }
        }

        private static Version CurrentVersion()
        {
            var data = ResourceManager.ParseJsonResourceAs<GlobalConfigModel>("wpex-global-config.data");
            return Version.Parse(data.Version);
        }

        private  Version InstalledVersion()
        {
            var data = Api.GetModFile("wpex-global-config.data").ParseJsonAsObject<GlobalConfigModel>();
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
                Api.ShowChatMessage(LangEx.Error("InvalidArgument"));
            }
        }

        public override string HelpText()
        {
            var sb = new StringBuilder();
            sb.AppendLine(LangEx.Meta("ModTitle"));
            sb.AppendLine("");
            sb.AppendLine(LangEx.Meta("ModDescription"));
            sb.AppendLine("");
            sb.AppendLine(LangEx.FluentChat(this, "SyntaxMessage_Full", SyntaxList));
            return sb.ToString();
        }
    }
}