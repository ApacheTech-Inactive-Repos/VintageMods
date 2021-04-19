using HarmonyLib;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming

namespace VintageMods.Mods.MinimalMapping.HarmonyPatches
{
    [HarmonyPatch(typeof(GuiElementMap), "ComposeElements")]
    internal class SetMiniMapZoomLevel
    {
        private static bool Prefix(ref GuiElementMap __instance)
        {
            __instance.ZoomLevel = 2f;
            return true;
        }
    }
}