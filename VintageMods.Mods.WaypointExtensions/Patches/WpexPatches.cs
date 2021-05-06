using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HarmonyLib;
using VintageMods.Core.Client.Extensions;
using VintageMods.Core.Common.Reflection;
using VintageMods.Core.FileIO.Extensions;
using VintageMods.Core.FluentChat.Exenstions;
using VintageMods.Mods.WaypointExtensions.Model;
using Vintagestory.API.Client;
using Vintagestory.API.Common.Entities;
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

        private static int JustTeleported { get; set; }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(BlockEntityStaticTranslocator), "OnEntityCollide")]
        private static bool Patch_BlockEntityStaticTranslocator_OnEntityCollide_Prefix(ref BlockEntityStaticTranslocator __instance, Entity entity)
        {
            if (JustTeleported > 0) return true;
            JustTeleported = 1;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000 * 30);
                JustTeleported = 0;
                Api.Logger.VerboseDebug("Translocator Waypoint Cooldown Reset.");
            });

            if (!Settings.AutoTranslocatorWaypoints) return true;
            if (entity != Api.World.Player.Entity) return true;
            if (!__instance.FullyRepaired || !__instance.Activated || !__instance.GetField<bool>("canTeleport"))
            {
                return false;
            }
            
            var sourcePos = __instance.Pos.RelativeToSpawn(Api.World);
            var destPos = __instance.TargetLocation.RelativeToSpawn(Api.World);

            if (Api.WaypointExistsWithinRadius(__instance.Pos, 5, p => p.Icon == "spiral")) return true;
            if (Api.WaypointExistsWithinRadius(__instance.TargetLocation, 5, p => p.Icon == "spiral")) return true;
            
            Api.AddWaypointAtPos(sourcePos, "spiral", "Fuchsia", 
                LangEx.Message("TranslocatorWaypoint",destPos.X, destPos.Y, destPos.Z), false);
            Api.Logger.VerboseDebug($"Added Waypoint: Translocator to ({destPos.X}, {destPos.Y}, {destPos.Z})");

            Api.AddWaypointAtPos(destPos, "spiral", "Fuchsia",
                LangEx.Message("TranslocatorWaypoint", sourcePos.X, sourcePos.Y, sourcePos.Z), false);
            Api.Logger.VerboseDebug($"Added Waypoint: Translocator to ({sourcePos.X}, {sourcePos.Y}, {sourcePos.Z})");
                
            return true;
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BlockEntityTeleporter), "OnEntityCollide")]
        private static bool Patch_BlockEntityTeleporter_OnEntityCollide_Prefix(
            ref BlockEntityTeleporter __instance, 
            ref Dictionary<long, TeleportingEntity> ___tpingEntities, 
            ref TeleporterLocation ___tpLocation)
        {
            // Still very hacky. It would be very nice to get a proper way of setting this waypoint.
            if (JustTeleported > 0) return true;
            if (!Settings.AutoTranslocatorWaypoints) return true;
            if (!___tpingEntities.ContainsKey(Api.World.Player.Entity.EntityId)) return true;
            if (Api.WaypointExistsWithinRadius(__instance.Pos, 3, p => p.Icon == "spiral")) return true;

            var title = LangEx.Message("TeleporterWaypoint", ___tpLocation?.TargetName ?? "Unknown");
            var sourcePos = __instance.Pos.RelativeToSpawn(Api.World);
            Api.AddWaypointAtPos(sourcePos, "spiral", "SpringGreen", title, false);
            Api.Logger.VerboseDebug($"Added Waypoint: {title}");

            JustTeleported = 1;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000 * 10);
                JustTeleported = 0;
                Api.Logger.VerboseDebug("Teleporter Waypoint Cooldown Reset.");
            });
            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(GuiDialogTrader), "OnGuiOpened")]
        private static void Patch_GuiDialogTrader_OnGuiOpened_Postfix()
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