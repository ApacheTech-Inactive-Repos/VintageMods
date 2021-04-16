using HarmonyLib;
using VintageMods.Core.Common.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming

namespace VintageMods.Accessibility.HarmonyPatches
{
    [HarmonyPatch(typeof(WeatherSimulationParticles), "Initialize")]
    internal class Override_WeatherSimulationParticles_Initialize
    {
        private static bool Prefix(ref WeatherSimulationParticles __instance)
        {
            var capi = __instance.GetField<ICoreClientAPI>("capi");
            __instance.SetField("lblock", capi.World.GetBlock(new AssetLocation("water-still-7")));
            return false;
        }
    }
}