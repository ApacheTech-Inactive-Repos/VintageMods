using VintageMods.Core.Attributes;
using VintageMods.Core.ModSystems;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

[assembly: ModDomain("cinecam", "CineCam")]
namespace VintageMods.Mods.CinematicCamStudio.ModSystems
{
    internal class CamSystem : ClientSideModSystem
    {
        public override void StartClientSide(ICoreClientAPI api)
        {

        }


        public override void Dispose()
        {               
            base.Dispose();
        }
    }
}
    