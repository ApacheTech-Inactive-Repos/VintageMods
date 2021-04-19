using System.Text;
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
    [HarmonyPatch(typeof(GuiDialogWorldMap), "OnMouseMove")]
    internal class DisableMapCoordsOnHover
    {
        private static bool Prefix(ref GuiDialogWorldMap __instance, MouseEvent args)
        {
            if (__instance.SingleComposer == null ||
                !__instance.SingleComposer.Bounds.PointInside(args.X, args.Y)) return false;

            var sb = new StringBuilder();

            var guiElementMap = (GuiElementMap) __instance.SingleComposer.GetElement("mapElem");
            var hoverText = __instance.SingleComposer.GetHoverText("hoverText");

            foreach (var mapLayer in guiElementMap.mapLayers)
            {
                mapLayer.OnMouseMoveClient(args, guiElementMap, sb);
            }

            hoverText.SetNewText(sb.ToString().TrimEnd());

            return false;
        }
    }
}