using System;
using JetBrains.Annotations;
using VintageMods.Core.ModSystems.Client;
using VintageMods.WaypointExtensions.Services;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

// ====================================================================================================
//  Future Implementation
// ====================================================================================================
//  
//  • Add new custom waypoint types from chat, saved to separate auto-generated json file.
//  • Potentially move away from json, and store everything in an sqlite db, including per-world data.
//  • Add GUI for more fine-tuned waypoint control.
//  • Add connected waypoints for Translocators.
//  • From VS-HUD: Floaty Waypoints.
//  • From VS-HUD: Add waypoint on death.
//  • Potential collab to consolidate codebases?
// ====================================================================================================

namespace VintageMods.WaypointExtensions.ModSystems
{
    /// <summary>
    ///     Entry Point for the Waypoint Extensions mod.
    ///     Implements <see cref="ClientSideModSystem" />
    /// </summary>
    /// <seealso cref="ClientSideModSystem{WaypointExtensionsService}" />
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature,
        ImplicitUseTargetFlags.WithMembers)]
    public class WaypointExtensionsModSystem : ClientSideModSystem<WaypointExtensionsService>
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The Vintage Story Core Client API.</param>
        public override void StartClientSide(ICoreClientAPI api)
        {
            api.RegisterCommand("wp",
                "Quickly, and easily add waypoint markers at your current position.",
                Service.InfoMessage(), Service.OnWpCommand);

            // Simple QoL commands.
            // TODO: Move QoL to separate mod.
            api.RegisterCommand("cls", "", "", (id, args) =>
            {
                for (var i = 0; i < 30; i++)
                {
                    api.ShowChatMessage("");
                }
            });
            api.RegisterCommand("chunks", "Shows/Hides chunk borders.", "", (id, args) =>
            {
                api.TriggerChatMessage(".debug wireframe chunk");
            });
            api.RegisterCommand("hitboxes", "Shows/Hides hit-boxes.", "", (id, args) =>
            {
                api.TriggerChatMessage(".debug wireframe entity");
            });

            // Quick implementation to test debug features.
            // TODO: v1.2.5 - Implement robust debugging feature, for scalable future development.
            api.RegisterCommand("wp-debug", "Waypoint Extensions Debugging Api", "", (id, args) =>
            {
                var action = args.PopWord("");
                switch (action)
                {
                    case "copy":
                        api.Forms.SetClipboardText(Service.InfoMessage());
                        api.ShowChatMessage("Waypoint Extensions: Syntax list copied to clipboard.");
                        return;

                    case "reload":
                        api.ShowChatMessage("Reloading Mod Files...");
                        Service.ReloadFiles();
                        return;
                }
            });
        }
    }
}