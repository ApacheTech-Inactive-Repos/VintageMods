using System;
using VintageMods.Core.ModSystems.Contracts;
using Vintagestory.API.Common;

namespace VintageMods.Core.ModSystems.Primitives
{
    /// <summary>
    ///     Base representation of a ModSystem used to extend Vintage Story.
    ///     Implements the <see cref="ModSystem" /> class.
    /// </summary>
    /// <typeparam name="TService">The type of the service to use for the business logic layer.</typeparam>
    /// <typeparam name="TApi">The type of the core game API to use for calls to and from the game.</typeparam>
    /// <seealso cref="ModSystem" />
    public abstract class ModSystemBase<TService, TApi> : ModSystem
        where TService : IServiceBase<TApi>
        where TApi : ICoreAPI
    {
        /// <summary>
        ///     Gets the service to use for the business logic layer.
        /// </summary>
        /// <value>The service to use for the business logic layer.</value>
        protected TService Service { get; private set; }

        /// <summary>
        ///     When the server reloads mods at runtime, should this mod also be reloaded. Return false e.g. for any mod that adds
        ///     blocks.
        /// </summary>
        /// <value><c>true</c> if runtime reload is allowed; otherwise, <c>false</c>.</value>
        public override bool AllowRuntimeReload { get; } = true;

        /// <summary>
        ///     Side agnostic Start method, called after all mods received a call to StartPre().
        /// </summary>
        /// <param name="api">The type of the core game API to use for calls to and from the game.</param>
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            Service = Activator.CreateInstance<TService>();
            Service?.OnStart((TApi) api);
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

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposeUnmanaged">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        ///     only unmanaged resources.
        /// </param>
        public virtual void Dispose(bool disposeUnmanaged)
        {
            Service?.Dispose(disposeUnmanaged);
            base.Dispose();
        }

        /// <summary>
        ///     Finalises an instance of the <see cref="ModSystemBase{TService,TApi}" /> class.
        /// </summary>
        ~ModSystemBase()
        {
            Dispose(false);
        }
    }
}