using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using VintageMods.Core.Network.Messages;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VintageMods.Core.ModSystems
{
    /// <summary>
    ///     Base representation of a universal ModSystem used to extend Vintage Story.
    ///     Implements the <see cref="ModSystem" /> class.
    /// </summary>
    public abstract class UniversalModSystem : ModSystemBase<ICoreAPI>
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="UniversalModSystem" /> class.
        /// </summary>
        protected UniversalModSystem(string id) : base(Assembly.GetCallingAssembly(), id)
        {
        }

        /// <summary>
        ///     Gets or sets the data packet used for MEF composition.
        /// </summary>
        /// <value>The data packet used for MEF composition.</value>
        [Import("CompositionData", AllowRecomposition = true)]
        public dynamic CompositionData { get; set; }

        /// <summary>
        ///     Gets the core client API.
        /// </summary>
        /// <value>The core client API.</value>
        public ICoreClientAPI Capi { get; private set; }

        /// <summary>
        ///     Gets the core server API.
        /// </summary>
        /// <value>The core server API.</value>
        public ICoreServerAPI Sapi { get; private set; }

        /// <summary>
        ///     Gets the client network channel.
        /// </summary>
        /// <value>The client network channel.</value>
        public IClientNetworkChannel ClientChannel { get; private set; }

        /// <summary>
        ///     Gets the server network channel.
        /// </summary>
        /// <value>The server network channel.</value>
        public IServerNetworkChannel ServerChannel { get; private set; }

        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The client side app api.</param>
        public abstract override void StartClientSide(ICoreClientAPI api);

        /// <summary>
        ///     Minor convenience method to save yourself the check for/cast to ICoreClientAPI in Start()
        /// </summary>
        /// <param name="api">The client side app api.</param>
        public abstract override void StartServerSide(ICoreServerAPI api);

        /// <summary>
        ///     Returns if this mod should be loaded for the given app side.
        /// </summary>
        /// <param name="forSide">Client Side, Server Side, or Universal.</param>
        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return true;
        }

        /// <summary>
        ///     Side agnostic Start method, called after all mods received a call to StartPre().
        /// </summary>
        /// <param name="api">The main API for the game.</param>
        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            switch (api.Side)
            {
                case EnumAppSide.Server:
                    ServerChannel = (Sapi ??= api as ICoreServerAPI)?.Network.RegisterChannel(Id)
                        .RegisterMessageType<CompositionDataPacket>()
                        .SetMessageHandler<CompositionDataPacket>(OnIncomingServerDataPacket);
                    break;
                case EnumAppSide.Client:
                    ClientChannel = (Capi ??= api as ICoreClientAPI)?.Network.RegisterChannel(Id)
                        .RegisterMessageType<CompositionDataPacket>()
                        .SetMessageHandler<CompositionDataPacket>(OnIncomingClientDataPacket);
                    break;
                case EnumAppSide.Universal:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     Called when an incoming data packet needs to be handled.
        /// </summary>
        /// <param name="player">The player that sent the packet.</param>
        /// <param name="packet">The incoming data packet to be handled.</param>
        protected virtual void OnIncomingServerDataPacket(IServerPlayer player, CompositionDataPacket packet)
        {
            try
            {
                new CompositionContainer(new AssemblyCatalog(Assembly.Load(packet.Data))).ComposeParts(this);
                CompositionData?.Compose(packet.Id ?? Id, player, Sapi);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        ///     Called when an incoming data packet needs to be handled.
        /// </summary>
        /// <param name="packet">The incoming data packet to be handled.</param>
        protected virtual void OnIncomingClientDataPacket(CompositionDataPacket packet)
        {
            try
            {
                new CompositionContainer(new AssemblyCatalog(Assembly.Load(packet.Data))).ComposeParts(this);
                CompositionData?.Compose(packet.Id ?? Id, Capi);
            }
            catch
            {
                // ignored
            }
        }
    }
}