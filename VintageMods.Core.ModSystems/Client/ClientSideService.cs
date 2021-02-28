using VintageMods.Core.ModSystems.Primitives;
using Vintagestory.API.Client;

// ReSharper disable UnusedType.Global

namespace VintageMods.Core.ModSystems.Client
{
    /// <summary>
    ///     Base class for Vintage Story client side mod services.
    ///     Class ClientSideService.
    ///     Implements the <see cref="ServiceBase{TApi}" />
    /// </summary>
    /// <seealso cref="ServiceBase{ICoreClientAPI}" />
    public abstract class ClientSideService : ServiceBase<ICoreClientAPI>
    {
        /***
         * Allows for client side only helper methods and shared functionality.
         **/
    }
}