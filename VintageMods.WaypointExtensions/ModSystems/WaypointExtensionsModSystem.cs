﻿using VintageMods.Core.ModSystems.Client;
using VintageMods.WaypointExtensions.Services;
using Vintagestory.API.Client;

namespace VintageMods.WaypointExtensions.ModSystems
{
    /// <summary>
    ///     Entry Point for the Waypoint Extensions mod.
    ///     Implements <see cref="ClientSideModSystem" />
    /// </summary>
    /// <seealso cref="ClientSideModSystem{WaypointExtensionsService}" />
    public class WaypointExtensionsModSystem : ClientSideModSystem<WaypointExtensionsService>
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The Vintage Story Core Client API.</param>
        public override void StartClientSide(ICoreClientAPI api)
        {
            api.RegisterCommand("wp",
                "Adds a waypoint to the World Map at the player's current position.",
                $"[{Service.SyntaxList}]",
                Service.AddWaypointCommandHandler);

            api.RegisterCommand("wp-debug", "", "", (id, args) =>
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