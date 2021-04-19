using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local
// ReSharper disable SuggestBaseTypeForParameter
// ReSharper disable InconsistentNaming

namespace VintageMods.Mods.MinimalMapping.HarmonyPatches
{
    [HarmonyPatch(typeof(GuiDialogEditWayPoint), "onPinnedToggled")]
    internal class DisableEditWaypointPinning
    {
        private static bool Prefix(ref GuiDialogEditWayPoint __instance, bool t1)
        {
            if (t1) __instance.SingleComposer.GetSwitch("pinnedSwitch").SetValue(false);
            return false;
        }
    }
}
