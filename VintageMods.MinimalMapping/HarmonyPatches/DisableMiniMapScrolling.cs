using HarmonyLib;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming

namespace VintageMods.MinimalMapping.HarmonyPatches
{
    [HarmonyPatch(typeof(GuiElementMap), "OnMouseDownOnElement")]
    internal class DisableMiniMapScrolling
    {
        private static bool Prefix(ref GuiElementMap __instance)
        {
            return false;
        }
        
    }
}