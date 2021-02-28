using VintageMods.Core.ModSystems.Primitives;
using Vintagestory.API.Server;

namespace VintageMods.Core.ModSystems.Server
{
    /// <summary>
    ///     Base class for Vintage Story server side mod services.
    ///     Implements the <see cref="ServiceBase{TApi}" />
    /// </summary>
    /// <seealso cref="ServiceBase{ICoreServerAPI}" />
    public abstract class ServerSideService : ServiceBase<ICoreServerAPI>
    {
        /***
         * Allows for server side only helper methods and shared functionality.
         **/
    }
}