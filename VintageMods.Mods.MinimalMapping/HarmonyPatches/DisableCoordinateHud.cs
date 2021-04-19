using HarmonyLib;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming

namespace VintageMods.Mods.MinimalMapping.HarmonyPatches
{
    [HarmonyPatch(typeof(GuiCompositeSettings), "onCoordinateHudChanged")]
    internal class DisableCoordinateHud
    {
        private static bool Prefix(bool on)
        {
            ClientSettings.ShowCoordinateHud = false;
            return false;
        }
    }
}