using VintageMods.Core.ModSystems.Primitives;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// ReSharper disable UnusedType.Global

namespace VintageMods.Core.ModSystems.Client
{
    /// <summary>
    ///     Base representation of a client side ModSystem used to extend Vintage Story.
    ///     Implements <see cref="ModSystemBase{TService,TApi}" />
    /// </summary>
    /// <typeparam name="TService">The type of the t service.</typeparam>
    /// <seealso cref="ModSystemBase{TService, ICoreClientAPI}" />
    public abstract class ClientSideModSystem<TService> : ModSystemBase<TService, ICoreClientAPI>
        where TService : ClientSideService
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The client side app api.</param>
        public abstract override void StartClientSide(ICoreClientAPI api);

        /// <summary>
        ///     Returns if this mod should be loaded for the given app side.
        /// </summary>
        /// <param name="forSide">Client Side, Server Side, or Universal.</param>
        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Client;
        }
    }

    /// <summary>
    ///     Base representation of a client side ModSystem used to extend Vintage Story.
    /// </summary>
    public abstract class ClientSideModSystem : ModSystem
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The client side app api.</param>
        public abstract override void StartClientSide(ICoreClientAPI api);

        /// <summary>
        ///     Returns if this mod should be loaded for the given app side.
        /// </summary>
        /// <param name="forSide">Client Side, Server Side, or Universal.</param>
        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Client;
        }

        /// <summary>
        ///     If you need mods to be executed in a certain order, adjust this methods return value.
        ///     The server will call each Mods Start() method the ascending order of each mods execute order value.
        ///     And thus, as long as every mod registers it's event handlers in the Start() method, all event handlers
        ///     will be called in the same execution order.
        ///     Default execute order of some survival mod parts.
        ///     World Gen:
        ///     - GenTerra: 0
        ///     - RockStrata: 0.1
        ///     - Deposits: 0.2
        ///     - Caves: 0.3
        ///     - Blocklayers: 0.4
        ///     Asset Loading
        ///     - Json Overrides loader: 0.05
        ///     - Load hardcoded mantle block: 0.1
        ///     - Block and Item Loader: 0.2
        ///     - Recipes (Smithing, Knapping, Clayforming, Grid recipes, Alloys) Loader: 1
        /// </summary>
        public override double ExecuteOrder()
        {
            return 0.05;
        }
    }
}