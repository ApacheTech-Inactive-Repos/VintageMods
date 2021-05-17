using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HarmonyLib;
using VintageMods.Core.Client.Extensions;
using VintageMods.Core.Common.Extensions;
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

            // Add Source TL Waypoint.
            if (!Api.WaypointExistsAtPos(__instance.Pos, p => p.Icon == "spiral"))
            {
                Api.AddWaypointAtPos(sourcePos, "spiral", "Fuchsia",
                    LangEx.Message("TranslocatorWaypoint", destPos.X, destPos.Y, destPos.Z), false);
                Api.Logger.VerboseDebug($"Added Waypoint: Translocator to ({destPos.X}, {destPos.Y}, {destPos.Z})");
            }

            // Add Destination TL Waypoint.
            if (!Api.WaypointExistsAtPos(__instance.TargetLocation, p => p.Icon == "spiral"))
            {
                Api.AddWaypointAtPos(destPos, "spiral", "Fuchsia",
                    LangEx.Message("TranslocatorWaypoint", sourcePos.X, sourcePos.Y, sourcePos.Z), false);
                Api.Logger.VerboseDebug($"Added Waypoint: Translocator to ({sourcePos.X}, {sourcePos.Y}, {sourcePos.Z})");
            }

            return true;
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BlockEntityTeleporter), "OnEntityCollide")]
        private static bool Patch_BlockEntityTeleporter_OnEntityCollide_Prefix(
            ref BlockEntityTeleporter __instance, 
            ref Dictionary<long, TeleportingEntity> ___tpingEntities, 
            ref TeleporterLocation ___tpLocation)
        {
            // TODO: Still very hacky. It would be very nice to get a proper way of setting this waypoint.
            //
            // Developer's Notes:   As of Game Version 1.14.10. There is currently no way to gather information
            //                      about the target side of a Teleporter block, other than its name, from the
            //                      Client API. In order for this to work, I would need to trick the server into
            //                      sending the client the updated list of teleporters on the server.
            //
            //                      This should be possible by mimicking the action of refreshing the GUI dialogue
            //                      without the dialogue box needing to be opened. However, parts of the dialogue
            //                      logic is locked behind a GameMode check. I could, internally switch the player
            //                      to creative and back, purely for this check, and it might even be possible to
            //                      do this check at player login, before the user has control of the player character.
            //
            //                      I'd then store the list of teleporters in memory, ready for use, if needed.

            if (JustTeleported > 0) return true;
            if (!Settings.AutoTranslocatorWaypoints) return true;
            if (!___tpingEntities.ContainsKey(Api.World.Player.Entity.EntityId)) return true;

            // Add Source TP Waypoint.
            if (Api.WaypointExistsAtPos(__instance.Pos, p => p.Icon == "spiral")) return true;
            var title = LangEx.Message("TeleporterWaypoint", 
                ___tpLocation?.TargetName?.IfNullOrWhitespace("Unknown"));
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
    }
}