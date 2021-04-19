using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming

namespace VintageMods.Mods.MinimalMapping.HarmonyPatches
{
    [HarmonyPatch(typeof(WaypointMapLayer), "AddWp")]
    internal class OverrideAddWp
    {
        private static bool Prefix(ref WaypointMapLayer __instance, Vec3d pos, CmdArgs args, IServerPlayer player, int groupId, string icon, bool pinned)
        {
            if (args.Length == 0)
            {
                player.SendMessage(groupId, Lang.Get("command-waypoint-syntax"), EnumChatType.CommandError);
                return false;
            }

            var colorstring = args.PopWord();
            var title = args.PopAll();

            Color parsedColor;

            if (colorstring.StartsWith("#"))
            {
                try
                {
                    var argb = int.Parse(colorstring.Replace("#", ""), NumberStyles.HexNumber);
                    parsedColor = Color.FromArgb(argb);
                }
                catch (FormatException)
                {
                    player.SendMessage(groupId, Lang.Get("command-waypoint-invalidcolor"), EnumChatType.CommandError);
                    return false;
                }
            }
            else
            {
                parsedColor = Color.FromName(colorstring);
            }

            if (string.IsNullOrEmpty(title))
            {
                player.SendMessage(groupId, Lang.Get("command-waypoint-notext"), EnumChatType.CommandError);
                return false;
            }
            var waypoint = new Waypoint()
            {
                Color = parsedColor.ToArgb() | (255 << 24),
                OwningPlayerUid = player.PlayerUID,
                Position = pos,
                Title = title,
                Icon = icon,
                Pinned = false
            };


            __instance.Waypoints.Add(waypoint);

            var ownwpaypoints = __instance.Waypoints.Where((p) => p.OwningPlayerUid == player.PlayerUID).ToArray();

            player.SendMessage(groupId, Lang.Get("Ok, waypoint nr. {0} added", ownwpaypoints.Length - 1), EnumChatType.CommandSuccess);
            AccessTools.Method(__instance.GetType(), "ResendWaypoints")?.Invoke(__instance, new object[]{ player });
            return false;
        }
    }
}