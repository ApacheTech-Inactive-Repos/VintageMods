using System.Reflection;
using JetBrains.Annotations;
using VintageMods.Core.Extensions;
using VintageMods.Core.FluentChat.Extensions;
using VintageMods.Core.Helpers;
using VintageMods.Core.Threading;
using VintageMods.Core.Threading.Extensions;
using VintageMods.Core.Threading.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VintageMods.Core.ModSystems
{
    /// <summary>
    ///     Base representation of a universal ModSystem used to extend Vintage Story.
    ///     Includes internal ClientSystems and ServerSystems, that are injected into the game, at runtime.
    ///     Implements the <see cref="ModSystem" /> class.
    /// </summary>
    /// <typeparam name="TServerSystem">The type of ServerSystem to inject into the game.</typeparam>
    /// <typeparam name="TClientSystem">The type of ClientSystem to inject into the game.</typeparam>
    /// <seealso cref="UniversalModSystem" />
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public abstract class UniversalInternalMod<TServerSystem, TClientSystem> : UniversalModSystem
        where TServerSystem : ServerSystemAsyncActions
        where TClientSystem : ClientSystemAsyncActions
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="UniversalInternalMod{TServerSystem, TClientSystem}" /> class.
        /// </summary>
        /// <param name="id">The mod-id.</param>
        protected UniversalInternalMod(string id) : base(id, Assembly.GetCallingAssembly())
        {
        }

        /// <summary>
        ///     Gets the injected server system, capable of running asynchronous actions.
        /// </summary>
        /// <value>An instance of the <see cref="TServerSystem" /> class.</value>
        [CanBeNull]
        public TServerSystem ServerInternal { get; private set; }

        /// <summary>
        ///     Gets the injected client system, capable of running asynchronous actions.
        /// </summary>
        /// <value>An instance of the <see cref="TClientSystem" /> class.</value>
        [CanBeNull]
        public TClientSystem ClientInternal { get; private set; }

        /// <summary>
        ///     Called during initial mod loading, called before any mod receives the call to Start()
        /// </summary>
        /// <param name="api">The API.</param>
        public override void StartPre(ICoreAPI api)
        {
            base.StartPre(api);

            if (api.Side.IsClient())
            {
                var capi = (ICoreClientAPI) api;
                capi.Event.LevelFinalize += () =>
                {
                    var system = ActivatorEx.CreateInstance<TClientSystem>(capi.AsClientMain());
                    if (!capi.IsClientSystemLoaded<TClientSystem>())
                        capi.InjectClientThread($"{Id}-client", system);
                    AsyncEx.Client = ClientInternal = capi.GetVanillaClientSystem<TClientSystem>();
                };
            }

            if (api.Side.IsServer())
            {
                var sapi = (ICoreServerAPI) api;
                sapi.Event.ServerRunPhase(EnumServerRunPhase.ModsAndConfigReady, () =>
                {
                    var system = ActivatorEx.CreateInstance<TServerSystem>(sapi.AsServerMain());
                    if (!sapi.IsServerSystemLoaded<TServerSystem>())
                        sapi.InjectServerThread($"{Id}-server", system);
                    AsyncEx.Server = ServerInternal = sapi.GetVanillaServerSystem<TServerSystem>();
                });
            }
        }

        public override void Dispose()
        {
            ClientInternal?.Dispose(ApiEx.Client);
            ServerInternal?.Dispose();
            base.Dispose();
        }
    }
}