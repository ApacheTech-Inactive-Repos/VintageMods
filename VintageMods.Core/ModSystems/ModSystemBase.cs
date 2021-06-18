using System.Reflection;
using HarmonyLib;
using JetBrains.Annotations;
using VintageMods.Core.IO;
using VintageMods.Core.IO.Extensions;
using Vintagestory.API.Common;

namespace VintageMods.Core.ModSystems
{
    /// <summary>
    ///     Base representation of a ModSystem used to extend Vintage Story.
    /// </summary>
    /// <typeparam name="TApi">The type of the API.</typeparam>
    /// <seealso cref="ModSystem" />
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers|ImplicitUseTargetFlags.WithInheritors)]
    public abstract class ModSystemBase<TApi> : ModSystem where TApi : class, ICoreAPI
    {
        private readonly Assembly _patchAssembly;

        /// <summary>
        ///     Gets the Harmony instance used to add patches for this mod.
        /// </summary>
        /// <value>The Harmony instance used to add patches for this mod.</value>
        protected Harmony ModPatches { get; }

        /// <summary>
        ///     Gets the file manager used to file system IO Operations.
        /// </summary>
        /// <value>The file manager.</value>
        protected FileManager Files { get; private set; }

        /// <summary>
        ///     Initialises a new instance of the <see cref="ModSystemBase{TApi}"/> class.
        /// </summary>
        protected ModSystemBase()
        {
            _patchAssembly = Assembly.GetExecutingAssembly();
            ModPatches = new Harmony(_patchAssembly.FullName);
        }

        /// <summary>
        ///     The main API for the game.
        /// </summary>
        /// <value>The main API for the game.</value>
        protected TApi Api { get; private set; }

        /// <summary>
        ///     Side agnostic Start method, called after all mods received a call to StartPre().
        /// </summary>
        /// <param name="api">The main API for the game.</param>
        public override void Start(ICoreAPI api)
        {
            Api = api as TApi;
            Files = api.RegisterFileManager();
            ApplyHarmonyPatches();
            api.Logger.Notification($"  {_patchAssembly.GetName()} - Patched Methods:");
            foreach (var val in ModPatches.GetPatchedMethods())
            {
                api.Logger.Notification("    " + val.FullDescription());
            }
        }

        /// <summary>
        ///     Applies the harmony patches for this mod.
        /// </summary>
        protected virtual void ApplyHarmonyPatches()
        {
            ModPatches.PatchAll(_patchAssembly);
        }

        /// <summary>
        ///     If you need mods to be executed in a certain order, adjust this methods return value.
        ///     The server will call each Mods Start() method the ascending order of each mods execute order value.
        ///     And thus, as long as every mod registers it's event handlers in the Start() method, all event handlers
        ///     will be called in the same execution order.
        /// 
        ///     Default execute order of some survival mod parts.
        /// 
        ///     World Gen:
        /// 
        ///     - GenTerra: 0
        ///     - RockStrata: 0.1
        ///     - Deposits: 0.2
        ///     - Caves: 0.3
        ///     - BlockLayers: 0.4
        /// 
        ///     Asset Loading:
        /// 
        ///     - Json Overrides loader: 0.05
        ///     - Load hardcoded mantle block: 0.1
        ///     - Block and Item Loader: 0.2
        ///     - Recipes (Smithing, Knapping, ClayForming, Grid recipes, Alloys) Loader: 1
        /// </summary>
        public override double ExecuteOrder()
        {
            return 0.05;
        }

        /// <summary>
        ///     If this mod allows runtime reloading, you must implement this method to unregister any listeners / handlers.
        /// </summary>
        public override void Dispose()
        {
            ModPatches.UnpatchAll(_patchAssembly.FullName);
            base.Dispose();
        }
    }
}
