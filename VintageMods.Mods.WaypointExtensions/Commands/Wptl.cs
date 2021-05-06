using VintageMods.Core.FluentChat.Attributes;
using VintageMods.Core.FluentChat.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable ClassNeverInstantiated.Global

namespace VintageMods.Mods.WaypointExtensions.Commands
{
    [FluentChatCommand("wptl")]
    internal class Wptl : FluentChatCommandBase<ICoreClientAPI>
    {
        public Wptl(ICoreClientAPI api) : base(api) { }

        public override void OnNoOption(string option, CmdArgs args)
        {
            AddTranslocatorWaypoint(false);
        }

        [FluentChatOption("pin")]
        private void OnPinnedOption(string option, CmdArgs args)
        {
            AddTranslocatorWaypoint(true);
        }

        private void AddTranslocatorWaypoint(bool pinned)
        {
            // Find nearby BlockStaticTranslocator or BlockTeleporter.
            // Determine position.
            // Is there a waypoint at that position?
            // Gather relevant details.
            // Add waypoint.
        }
    }
}
