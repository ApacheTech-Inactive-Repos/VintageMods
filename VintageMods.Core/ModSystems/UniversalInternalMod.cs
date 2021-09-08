using VintageMods.Core.Extensions;
using VintageMods.Core.FluentChat.Extensions;
using VintageMods.Core.Threading;
using VintageMods.Core.Threading.Extensions;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

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
    public abstract class UniversalInternalMod<TServerSystem, TClientSystem> : UniversalModSystem
        where TServerSystem : ServerSystem
        where TClientSystem : ClientSystem
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="UniversalInternalMod{TServerSystem, TClientSystem}"/> class.
        /// </summary>
        /// <param name="id">The mod-id.</param>
        protected UniversalInternalMod(string id) : base(id) { }

        /// <summary>
        ///     Called during initial mod loading, called before any mod receives the call to Start()
        /// </summary>
        /// <param name="api">The API.</param>
        public override void StartPre(ICoreAPI api)
        {
            base.StartPre(api);

            if (api.Side.IsClient())
            {
                var capi = (ICoreClientAPI)api;
                capi.Event.LevelFinalize += () =>
                {
                    if (!capi.IsClientSystemLoaded<TClientSystem>())
                        capi.InjectClientThread($"{Id}-client",
                            ActivatorEx.CreateInstance<TClientSystem>(capi.AsClientMain()));
                };
            }

            if (api.Side.IsServer())
            {
                var sapi = (ICoreServerAPI)api;
                sapi.Event.ServerRunPhase(EnumServerRunPhase.ModsAndConfigReady, () =>
                {
                    if (!sapi.IsServerSystemLoaded<TServerSystem>())
                        sapi.InjectServerThread($"{Id}-server",
                            ActivatorEx.CreateInstance<TServerSystem>(sapi.AsServerMain()));
                });
            }
        }
    }
}