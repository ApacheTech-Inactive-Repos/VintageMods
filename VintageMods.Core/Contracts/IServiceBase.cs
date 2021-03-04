using JetBrains.Annotations;
using Vintagestory.API.Common;

namespace VintageMods.Core.Contracts
{
    /// <summary>
    ///     Represents a concrete mod service, forming an abstraction layer for a ModSystem.
    /// </summary>
    /// <typeparam name="TApi">The type of the API to use for internal game functionality.</typeparam>
    [UsedImplicitly(
        ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature,
        ImplicitUseTargetFlags.WithMembers)]
    public interface IServiceBase<TApi> where TApi : ICoreAPI
    {
        /// <summary>
        ///     Gets the core game API.
        /// </summary>
        /// <value>The core game API.</value>
        TApi Api { get; }

        /// <summary>
        ///     Called when the Start method of the ModSystem is called.
        /// </summary>
        /// <param name="api">The API.</param>
        void OnStart(TApi api);

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void Dispose();

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposeUnmanaged">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release
        ///     only unmanaged resources.
        /// </param>
        void Dispose(bool disposeUnmanaged);
    }
}