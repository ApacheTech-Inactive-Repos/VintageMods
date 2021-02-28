using VintageMods.Core.ModSystems.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VintageMods.Core.ModSystems.Universal
{
    // Marker interfaces: Code smell???

    public interface IUniversalService : IServiceBase<ICoreAPI>
    {
    }

    public interface IClientSideService : IServiceBase<ICoreClientAPI>
    {

    }

    public interface IServerSideService : IServiceBase<ICoreServerAPI>
    {
    }
}