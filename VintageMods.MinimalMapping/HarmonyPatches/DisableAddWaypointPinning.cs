using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming

namespace VintageMods.MinimalMapping.HarmonyPatches
{
    [HarmonyPatch(typeof(GuiDialogAddWayPoint), "onPinnedToggled")]
    internal class DisableAddWaypointPinning
    {
        private static bool Prefix(ref GuiDialogAddWayPoint __instance, bool on)
        {
            if (on) __instance.SingleComposer.GetSwitch("pinnedSwitch").SetValue(false);
            return false;
        }
    }
}