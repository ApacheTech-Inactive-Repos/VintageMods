using System;
using VintageMods.Core.ModSystems.Contracts;
using VintageMods.Core.ModSystems.IO;
using Vintagestory.API.Common;

namespace VintageMods.Core.ModSystems.Primitives
{
    /// <summary>
    ///     Base class for Vintage Story Mod Services.
    /// </summary>
    /// <typeparam name="TApi">The type of the API to use within the service.</typeparam>
    public abstract class ServiceBase<TApi> : IDisposable, IServiceBase<TApi> where TApi : ICoreAPI
    {
        /// <summary>
        ///     Gets the name of the root folder used by the mod.
        /// </summary>
        /// <value>The name of the root folder used by the mod.</value>
        public abstract string RootFolder { get; }

        /// <summary>
        ///     Provides access to files and folders the mod needs to store persistent data.
        /// </summary>
        /// <value>The file manager used to access files and folders the mod needs to store persistent data.</value>
        protected FileManager ModFiles { get; private set; }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gets the core game API.
        /// </summary>
        /// <value>The core game API.</value>
        public TApi Api { get; private set; }

        /// <summary>
        ///     Called when the Start method of the ModSystem is called.
        /// </summary>
        /// <param name="api">The API.</param>
        public virtual void OnStart(TApi api)
        {
            Api = api;
            ModFiles = new FileManager(api, RootFolder);
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
        }

        /// <summary>
        ///     Finalises an instance of the <see cref="ServiceBase{TApi}" /> class.
        /// </summary>
        ~ServiceBase()
        {
            Dispose(false);
        }
    }
}