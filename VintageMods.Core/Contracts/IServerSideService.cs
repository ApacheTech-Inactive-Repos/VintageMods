using Vintagestory.API.Server;

namespace VintageMods.Core.Contracts
{
    /// <summary>
    ///     Represents a server side mod service, forming an abstraction layer for a ModSystem.
    ///     Implements the <see cref="IServiceBase{ICoreServerAPI}" />
    /// </summary>
    /// <seealso cref="IServiceBase{ICoreServerAPI}" />
    public interface IServerSideService : IServiceBase<ICoreServerAPI>
    {
        /* Marker Interface: Considered a code smell, if not populated while scaling. */
    }
}