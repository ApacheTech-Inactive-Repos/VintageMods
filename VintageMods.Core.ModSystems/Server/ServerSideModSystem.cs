using VintageMods.Core.ModSystems.Contracts;
using VintageMods.Core.ModSystems.Primitives;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VintageMods.Core.ModSystems.Server
{
    /// <summary>
    ///     Base representation of a server side ModSystem used to extend Vintage Story.
    ///     Implements the <see cref="ModSystemBase{TService,TApi}" />
    /// </summary>
    /// <typeparam name="TService">The type of the t service.</typeparam>
    /// <seealso cref="ModSystemBase{TService, ICoreServerAPI}" />
    public abstract class ServerSideModSystem<TService> : ModSystemBase<TService, ICoreServerAPI>
        where TService : IServerSideService
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The server side app api.</param>
        public abstract override void StartServerSide(ICoreServerAPI api);

        /// <summary>
        ///     Returns if this mod should be loaded for the given app side.
        /// </summary>
        /// <param name="forSide">Client Side, Server Side, or Universal.</param>
        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Server;
        }
    }
}