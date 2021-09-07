using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VintageMods.Core.ModSystems
{
    /// <summary>
    ///     Base representation of a server side ModSystem used to extend Vintage Story.
    ///     Implements the <see cref="ModSystem" />
    /// </summary>
    /// <seealso cref="ModSystem" />
    public abstract class ServerSideModSystem : ModSystemBase<ICoreServerAPI>
    {
        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreServerAPI in Start()
        /// </summary>
        /// <param name="api">The server side app API.</param>
        public abstract override void StartServerSide(ICoreServerAPI api);

        /// <summary>
        ///     Returns if this mod should be loaded for the given app side.
        /// </summary>
        /// <param name="forSide">Client Side, Server Side, or Universal.</param>
        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Server;
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="ServerSideModSystem" /> class.
        /// </summary>
        /// <param name="assembly">The mod assembly used to create this instance.</param>
        protected ServerSideModSystem(string id) : base(Assembly.GetCallingAssembly(), id)
        {
        }
    }
}