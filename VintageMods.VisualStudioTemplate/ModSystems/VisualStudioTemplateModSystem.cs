using VintageMods.Core.ModSystems.Client;
using VintageMods.VisualStudioTemplate.Services;
using Vintagestory.API.Client;

namespace VintageMods.VisualStudioTemplate.ModSystems
{
    public sealed class VisualStudioTemplateModSystem : ClientSideModSystem<VisualStudioTemplateService>
    {
        public override void StartClientSide(ICoreClientAPI api)
        {
            api.RegisterCommand("say-hello", "Displays a welcome message in chat.", "", Service.OnSayHelloCommand);
        }
    }
}