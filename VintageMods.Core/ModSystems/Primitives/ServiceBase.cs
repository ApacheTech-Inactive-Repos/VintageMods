using System;
using JetBrains.Annotations;
using VintageMods.Core.Contracts;
using VintageMods.Core.IO;
using Vintagestory.API.Common;

namespace VintageMods.Core.ModSystems.Primitives
{
    /// <summary>
    ///     Base class for Vintage Story Mod Services.
    /// </summary>
    /// <typeparam name="TApi">The type of the API to use within the service.</typeparam>
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature,
        ImplicitUseTargetFlags.WithMembers)]
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
        public FileManager FileSystem { get; private set; }

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
            FileSystem = new FileManager(api, RootFolder);
        }

        #region Implementation of IDisposable Pattern

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
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
            /***
             * Empty virtual methods can denote a code smell, however, this early in development,
             * it's not too much of an issue. It could be said that if it's not needed for every
             * mod, it shouldn't be in the base class, but something like graceful degradation is
             * so important that having a robust, and fully integrated system in place, if needed,
             * is likely the best way to go.
             *
             * Based on the structure of the generic base classes, this was the best way to implement
             * the Disposable pattern, in its most elegant form. Any other way would have been a much
             * more piecemeal approach, which would means only implementing half of the pattern here,
             * and half as and when it's needed within derived classes. This could cause serious
             * issues if not handled correctly, later on down the line. It also wouldn't be fair for
             * every mod to include a Dispose method, by making this abstract, because it just
             * passes this marker method problem down the line, and replicates it within every mod
             * that doesn't need manual graceful degradation.
             **/
        }

        /// <summary>
        ///     Finalises an instance of the <see cref="ServiceBase{TApi}" /> class.
        /// </summary>
        ~ServiceBase()
        {
            Dispose(false);
        }

        #endregion
    }
}