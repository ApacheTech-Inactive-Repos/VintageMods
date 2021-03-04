using Vintagestory.API.Client;

namespace VintageMods.Core.Contracts
{
    /// <summary>
    ///     Represents a client side mod service, forming an abstraction layer for a ModSystem.
    ///     Implements the <see cref="IServiceBase{ICoreClientAPI}" />
    /// </summary>
    /// <seealso cref="IServiceBase{ICoreClientAPI}" />
    public interface IClientSideService : IServiceBase<ICoreClientAPI>
    {
        /* Marker Interface: Considered a code smell, if not populated while scaling. */
    }
}