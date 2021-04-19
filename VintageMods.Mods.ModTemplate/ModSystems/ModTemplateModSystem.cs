using VintageMods.Core.Client.ModSystems;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

namespace VintageMods.Mods.ModTemplate.ModSystems
{
    internal class ModTemplateModSystem : ClientSideModSystem
    {
        public override void StartClientSide(ICoreClientAPI api)
        {
            api.RegisterCommand("ModTemplate", "", "", (id, args) =>
            {
                api.ShowChatMessage("ModTemplate is working correctly.");
            });
        }
    }
}