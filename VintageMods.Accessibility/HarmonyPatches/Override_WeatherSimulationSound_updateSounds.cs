using HarmonyLib;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming

namespace VintageMods.Accessibility.HarmonyPatches
{
    [HarmonyPatch(typeof(WeatherSimulationSound), "updateSounds")]
    internal class Override_WeatherSimulationSound_updateSounds
    {
        private static bool Prefix()
        {
            return false;
        }
    }
}