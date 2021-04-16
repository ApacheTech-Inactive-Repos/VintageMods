using VintageMods.Core.Client.Extensions;
using VintageMods.Core.Client.ModSystems;
using VintageMods.Core.Client.Reflection;
using VintageMods.WaypointExtensions.ClientSystems;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

namespace VintageMods.WaypointExtensions.ModSystems
{
    internal class WpexModSystem : ClientSideModSystem
    {
        public override void StartClientSide(ICoreClientAPI api)
        {
            api.Event.LevelFinalize += () =>
            {
                api.InjectClientThread("wpex", 40, new WpexClientSystem(api.AsClientMain()));
            };
        }
    }
}
