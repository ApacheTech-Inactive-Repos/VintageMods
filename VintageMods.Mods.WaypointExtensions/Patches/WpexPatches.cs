using System.Collections.Generic;
using HarmonyLib;
using VintageMods.Core.Client.Extensions;
using VintageMods.Core.Common.Reflection;
using VintageMods.Core.FileIO.Extensions;
using VintageMods.Mods.WaypointExtensions.Model;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable InconsistentNaming

namespace VintageMods.Mods.WaypointExtensions.Patches
{
    [HarmonyPatch]
    public static class WpexPatches
    {
        internal static ICoreClientAPI Api { get; set; }

        internal static WorldSettings Settings =>
            Api?.GetModFile("wpex-settings.data").ParseJsonAsObject<WorldSettings>() ?? new WorldSettings();

        private static bool JustTeleported { get; set; }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(BlockEntityStaticTranslocator), "OnClientGameTick")]
        private static bool Patch_BlockEntityStaticTranslocator_OnClientGameTick_Prefix(ref BlockEntityStaticTranslocator __instance)
        {
            if (!__instance.GetField<bool>("somebodyIsTeleporting")) return true;
            if (!Settings.AutoTranslocatorWaypoints) return true;
            var entities = __instance.GetField<Dictionary<long, TeleportingEntity>>("tpingEntities");
            if (!entities.ContainsKey(Api.World.Player.Entity.EntityId)) return true;
            if (Api.World.ElapsedMilliseconds - __instance.GetField<long>("somebodyIsTeleportingReceivedTotalMs") > 30L) return true;

            var sourcePos = __instance.Pos.RelativeToSpawn(Api.World);
            var destPos = __instance.TargetLocation.RelativeToSpawn(Api.World);

            if (Api.WaypointExistsWithinRadius(sourcePos, 3, p => p.Icon == "spiral")) return true;
            Api.AddWaypointAtPos(sourcePos, "spiral", "Fuchsia", 
                $"Translocator to ({destPos.X}, {destPos.Y}, {destPos.Z})", false);
            Api.Logger.Audit($"Added Waypoint: Translocator to ({destPos.X}, {destPos.Y}, {destPos.Z})");

            if (Api.WaypointExistsWithinRadius(destPos, 3, p => p.Icon == "spiral")) return true;
            Api.AddWaypointAtPos(destPos, "spiral", "Fuchsia", 
                $"Translocator to ({sourcePos.X}, {sourcePos.Y}, {sourcePos.Z})", false);
            Api.Logger.Audit($"Added Waypoint: Translocator to ({sourcePos.X}, {sourcePos.Y}, {sourcePos.Z})");

            return true;
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(GuiDialogTrader), "OnGuiOpened")]
        private static void Patch_GuiDialogTrader_OnGuiOpened()
        {
            if (Settings.AutoTraderWaypoints)
            {
                Api.Event.EnqueueMainThreadTask(() =>
                    {
                        Api.TriggerChatMessage(".wpt");
                    },
                    "Patch_EntityTrader_OnInteract");
            }
        }
    }
}

















//[HarmonyPrefix]
//[HarmonyPatch(typeof(ElementBounds), "get_renderX")]
//private static bool PatchX(ref ElementBounds __instance, ref double __result)
//{
//    try
//    {
//        __result =
//            __instance.absFixedX +
//            __instance.absMarginX +
//            __instance.absOffsetX +
//            __instance.ParentBounds.absPaddingX +
//            __instance.ParentBounds.renderX +
//            __instance.renderOffsetX;
//    }
//    catch (NullReferenceException ex)
//    {
//        __result =
//            wpButtonBounds.absFixedX +
//            wpButtonBounds.absMarginX +
//            wpButtonBounds.absOffsetX +
//            wpButtonBounds.ParentBounds.absPaddingX +
//            wpButtonBounds.ParentBounds.renderX +
//            wpButtonBounds.renderOffsetX;
//    }
//    return false;
//}

//[HarmonyPrefix]
//[HarmonyPatch(typeof(ElementBounds), "get_renderY")]
//private static bool PatchY(ref ElementBounds __instance, ref double __result)
//{
//    try
//    {
//        __result =
//            __instance.absFixedY +
//            __instance.absMarginY +
//            __instance.absOffsetY +
//            __instance.ParentBounds.absPaddingY +
//            __instance.ParentBounds.renderY +
//            __instance.renderOffsetY;
//    }
//    catch (NullReferenceException ex)
//    {
//        __result =
//            wpButtonBounds.absFixedY +
//            wpButtonBounds.absMarginY +
//            wpButtonBounds.absOffsetY +
//            wpButtonBounds.ParentBounds.absPaddingY +
//            wpButtonBounds.ParentBounds.renderY +
//            wpButtonBounds.renderOffsetY;
//    }
//    return false;
//}

//private static ElementBounds wpButtonBounds = ElementBounds.Fixed(0.0, 0.0, 0.0, 40.0).WithFixedPadding(0.0, 3.0);

//private static bool? _toggleState;
//private static GuiCompositeSettings _gui;

//[HarmonyPostfix]
//[HarmonyPatch(typeof(GuiCompositeSettings), "updateButtonBounds")]
//private static void Patch_GuiCompositeSettings_updateButtonBounds_Postfix(ref GuiCompositeSettings __instance)
//{
//    var cairoFont = CairoFont.ButtonText();
//    var width = cairoFont.GetTextExtents(Lang.Get("setting-graphics-header")).Width / ClientSettings.GUIScale + 15.0;
//    var dButtonBounds = __instance.GetField<ElementBounds>("dButtonBounds");
//    wpButtonBounds.WithFixedWidth(width).FixedRightOf(dButtonBounds, 15.0);
//}

//[HarmonyPostfix]
//[HarmonyPatch(typeof(GuiCompositeSettings), "ComposerHeader")]
//private static void Patch_GuiCompositeSettings_ComposerHeader_Postfix(ref GuiCompositeSettings __instance, ref GuiComposer __result)
//{
//    _gui = __instance;
//    var font = CairoFont.ButtonText();
//    __instance.CallMethod("updateButtonBounds");

//    if (__instance.GetField<bool>("onMainscreen"))
//    {
//        var height = _w1hCRzQiukKlSxBw6hggDUXQ0jC._US2CIuLTGGHniL8z2evBmvYk3Kj.WindowSize.Height;
//        var elementBounds2 = ElementBounds.Fixed(50.0, GameMath.Clamp(height * 0.1, 20.0, 140.0), 750.0, 600.0);
//        wpButtonBounds.ParentBounds = elementBounds2;

//        Api.Logger.Audit("onMainscreen: Before");
//        __result.AddToggleButton(Lang.Get("setting-graphics-header"), font, OnWpSettingsToggle, wpButtonBounds, "wpex");
//        Api.Logger.Audit("onMainscreen: After");
//    }
//    else
//    {
//        var elementBounds4 = new ElementBounds().WithSizing(ElementSizing.FitToChildren).WithFixedPadding(GuiStyle.ElementToDialogPadding);
//        elementBounds4.horizontalSizing = ElementSizing.Fixed;
//        elementBounds4.fixedWidth = 900.0 - 2.0 * GuiStyle.ElementToDialogPadding;
//        wpButtonBounds.ParentBounds = elementBounds4;

//        Api.Logger.Audit("!onMainscreen: Before");
//        __result.AddToggleButton(Lang.Get("setting-graphics-header"), font, OnWpSettingsToggle, wpButtonBounds, "wpex");
//        Api.Logger.Audit("!onMainscreen: After");
//    }
//    __result.GetToggleButton("wpex").SetValue(_toggleState ?? false);
//    Api.Logger.Audit("ComposerHeader: After");
//}


//private static void OnWpSettingsToggle(bool state)
//{
//    _toggleState = state;
//    Api.Logger.Audit($"Wpex Menu State: {state}");
//    var guiComposer = _gui.CallMethod<GuiComposer>("ComposerHeader", "gamesettings-graphics", "wpex");
//}

