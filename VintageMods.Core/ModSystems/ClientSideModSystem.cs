﻿using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace VintageMods.Core.ModSystems
{
    /// <summary>
    ///     Base representation of a client side ModSystem used to extend Vintage Story.
    /// </summary>
    public abstract class ClientSideModSystem : ModSystemBase<ICoreClientAPI>
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="ClientSideModSystem" /> class.
        /// </summary>
        /// <param name="assembly">The mod assembly used to create this instance.</param>
        protected ClientSideModSystem(string id) : base(Assembly.GetCallingAssembly(), id)
        {
        }

        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The client side app API.</param>
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
}