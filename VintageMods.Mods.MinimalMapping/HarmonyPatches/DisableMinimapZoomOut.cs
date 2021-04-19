using HarmonyLib;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming

namespace VintageMods.Mods.MinimalMapping.HarmonyPatches
{
    [HarmonyPatch(typeof(GuiElementMap), "ZoomAdd")]
    internal class DisableMinimapZoomOut
    {
        private static bool Prefix(ref GuiElementMap __instance, float zoomDiff, float px, float pz)
        {
            __instance.ZoomLevel = GameMath.Clamp(__instance.ZoomLevel + zoomDiff, 2f, 6f);

            var nowRelSize = 1 / __instance.ZoomLevel;
            var diffX = __instance.Bounds.InnerWidth * nowRelSize - __instance.CurrentBlockViewBounds.Width;
            var diffZ = __instance.Bounds.InnerHeight * nowRelSize - __instance.CurrentBlockViewBounds.Length;

            __instance.CurrentBlockViewBounds.X2 += diffX;
            __instance.CurrentBlockViewBounds.Z2 += diffZ;

            __instance.CurrentBlockViewBounds.Translate(-diffX * px, 0, -diffZ * pz);

            __instance.EnsureMapFullyLoaded();
            return false;
        }
    }
}