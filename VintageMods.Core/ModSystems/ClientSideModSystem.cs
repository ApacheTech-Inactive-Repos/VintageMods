// ReSharper disable MemberCanBePrivate.Global

using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace VintageMods.Core.Client.ModSystems
{
    /// <summary>
    ///     Base representation of a client side ModSystem used to extend Vintage Story.
    /// </summary>
    public abstract class ClientSideModSystem : ModSystem
    {
        private readonly Assembly _patchAssembly;
        
        protected Harmony ModPatches { get; }

        protected ClientSideModSystem()
        {
            _patchAssembly = Assembly.GetExecutingAssembly();
            ModPatches = new Harmony(_patchAssembly.FullName);
        }

        /// <summary>
        ///     The main client side API for the game.
        /// </summary>
        /// <value>The main client side API for the game.</value>
        protected ICoreClientAPI Api { get; private set; }

        public override void Start(ICoreAPI api)
        {
            Api = api as ICoreClientAPI;
            ApplyHarmonyPatches();
            api.Logger.Notification($"{_patchAssembly.GetName()} - Patched Methods:");
            foreach (var val in ModPatches.GetPatchedMethods())
            {
                api.Logger.Notification("\t\t" + val.FullDescription());
            }
        }

        protected virtual void ApplyHarmonyPatches()
        {
            ModPatches.PatchAll(_patchAssembly);
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

        public override void Dispose()
        {
            ModPatches.UnpatchAll(_patchAssembly.FullName);
            base.Dispose();
        }
    }
}
