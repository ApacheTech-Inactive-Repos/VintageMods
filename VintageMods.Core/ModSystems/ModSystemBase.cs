using System;
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
        /// <summary>
        ///     Gets the mod name.
        /// </summary>
        /// <value>The name of the mod.</value>
        public string Id { get; }

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
        public ModSystemBase(Assembly assembly, string id)
        {
            _patchAssembly = assembly ?? Assembly.GetCallingAssembly();
            ModPatches = new Harmony(_patchAssembly.FullName);
            Id = id;
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
            ApplyHarmonyPatches(_patchAssembly);
            api.Logger.Notification($"  {_patchAssembly.GetName()} - Patched Methods:");
            foreach (var val in ModPatches.GetPatchedMethods())
            {
                api.Logger.Notification("    " + val.FullDescription());
            }
        }

        private byte _retries;
        /// <summary>
        ///     Applies the harmony patches for this mod.
        /// </summary>
        protected virtual void ApplyHarmonyPatches(Assembly assembly)
        {
            try
            {
                ModPatches.PatchAll(assembly);
            }
            catch (Exception ex)
            {
                Api.Logger.Audit($"{ex.Message}");
                ModPatches.PatchAll(assembly == Assembly.GetExecutingAssembly() ? Assembly.GetCallingAssembly() : Assembly.GetExecutingAssembly());
                if (++_retries == 3) throw;
            }
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
