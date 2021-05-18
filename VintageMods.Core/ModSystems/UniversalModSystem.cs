using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VintageMods.Core.ModSystems
{
    /// <summary>
    ///     Base representation of a universal ModSystem used to extend Vintage Story.
    ///     Implements the <see cref="ModSystem"/> class.
    /// </summary>
    public abstract class UniversalModSystem : ModSystemBase<ICoreAPI>
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The client side app api.</param>
        public abstract override void StartClientSide(ICoreClientAPI api);

        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The client side app api.</param>
        public abstract override void StartServerSide(ICoreServerAPI api);

        /// <summary>
        ///     Returns if this mod should be loaded for the given app side.
        /// </summary>
        /// <param name="forSide">Client Side, Server Side, or Universal.</param>
        public override bool ShouldLoad(EnumAppSide forSide) => true;
    }
}