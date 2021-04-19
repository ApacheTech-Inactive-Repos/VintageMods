using System;
using System.Collections.Generic;
using VintageMods.Mods.CinematicCamStudio.Camera.Targetting;

namespace VintageMods.Mods.CinematicCamStudio.Camera.Pathfinding
{
    internal class CamPathSettings
    {
        public static int LastLoop { get; set; }
        public static TimeSpan LastDuration { get; set; }
        public static string LastMode { get; set; }
        public static string LastInterpolation { get; set; }
        public static CamTarget Target { get; set; }
        public static List<CamNode> Points { get; set; }
        public static double CameraFollowSpeed { get; set; }
    }
}