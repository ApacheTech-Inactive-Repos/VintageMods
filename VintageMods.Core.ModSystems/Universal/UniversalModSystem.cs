using VintageMods.Core.ModSystems.Client;
using VintageMods.Core.ModSystems.Contracts;
using VintageMods.Core.ModSystems.Primitives;
using VintageMods.Core.ModSystems.Server;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

// ReSharper disable UnusedType.Global

namespace VintageMods.Core.ModSystems.Universal
{

    /// <summary>
    ///     Base representation of a universal ModSystem used to extend Vintage Story.
    ///     Implements the <see cref="ModSystemBase{TService,TApi}" />
    /// </summary>
    /// <typeparam name="TUniversalService">The service to use for both client and server side functions.</typeparam>
    /// <seealso cref="ModSystemBase{TUniversalService, ICoreAPI}" />
    public abstract class UniversalModSystem<TUniversalService> :
        ModSystemBase<TUniversalService, ICoreAPI>
        where TUniversalService : IUniversalService
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The server side app api.</param>
        public abstract override void StartClientSide(ICoreClientAPI api);

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
            return true;
        }
    }

    /// <summary>
    ///     Base representation of a universal ModSystem used to extend Vintage Story.
    ///     Implements the <see cref="ModSystemBase{TService,TApi}" />
    /// </summary>
    /// <typeparam name="TClientService">The service to use for client side functions.</typeparam>
    /// <typeparam name="TServerService">The service to use for server side functions.</typeparam>
    /// <seealso cref="ModSystemBase{TService, ICorerAPI}" />
    /// 
    public abstract class UniversalModSystem<TClientService, TServerService> : 
        ModSystemBase<UniversalService<TClientService, TServerService>, ICoreAPI>
        where TClientService : ClientSideService
        where TServerService : ServerSideService
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The server side app api.</param>
        public abstract override void StartClientSide(ICoreClientAPI api);

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
            return true;
        }
    }
}