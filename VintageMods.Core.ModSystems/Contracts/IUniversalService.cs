using Vintagestory.API.Common;

namespace VintageMods.Core.ModSystems.Contracts
{
    /// <summary>
    ///     Represents an ambi-sided mod service, forming an abstraction layer for a ModSystem.
    ///     Implements the <see cref="IServiceBase{ICoreAPI}" />
    /// </summary>
    /// <seealso cref="IServiceBase{ICoreAPI}" />
    public interface IUniversalService : IServiceBase<ICoreAPI>
    {
        /* Marker Interface: Considered a code smell, if not populated while scaling. */
    }
}