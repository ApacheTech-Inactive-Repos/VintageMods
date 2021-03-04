using JetBrains.Annotations;
using VintageMods.Core.Fluent.Extensions;
using VintageMods.Core.ModSystems.Client;
using VintageMods.WaypointExtensions.Services;
using VintageMods.WaypointExtensions.WpDebug;
using Vintagestory.API.Client;

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

            // .wp-debug Command.
            api.RegisterFluentCommand<WpDebugChatCommand, WaypointExtensionsService>(Service);
        }
    }
}