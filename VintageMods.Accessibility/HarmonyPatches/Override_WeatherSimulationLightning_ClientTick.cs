using HarmonyLib;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming

namespace VintageMods.Accessibility.HarmonyPatches
{
    [HarmonyPatch(typeof(WeatherSimulationLightning), "ClientTick")]
    internal class Override_WeatherSimulationLightning_ClientTick
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}