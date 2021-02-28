using System;
using VintageMods.Core.ModSystems.Client;
using VintageMods.Core.ModSystems.Primitives;
using VintageMods.Core.ModSystems.Server;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedType.Global

namespace VintageMods.Core.ModSystems.Universal
{

    /// <summary>
    ///     Base class for Vintage Story universal mod services.
    ///     Implements <see cref="ServiceBase{TApi}" />
    /// </summary>
    /// <seealso cref="ServiceBase{ICoreAPI}" />
    public abstract class UniversalService : ServiceBase<ICoreAPI>, IUniversalService
    {
        public abstract override void OnStart(ICoreAPI api);
    }

    /// <summary>
    ///     Base class for Vintage Story universal mod services.
    ///     Implements <see cref="ServiceBase{TApi}" />
    /// </summary>
    /// <seealso cref="ServiceBase{ICoreAPI}" />
    public abstract class UniversalService<TClientService, TServerService> : ServiceBase<ICoreAPI>, IUniversalService
        where TClientService : ClientSideService
        where TServerService : ServerSideService
    {
        /// <summary>
        ///     Gets the client-side service.
        /// </summary>
        /// <value>The client-side service.</value>
        public TClientService Client { get; private set; }

        /// <summary>
        ///     Gets the server-side service.
        /// </summary>
        /// <value>The server-side service.</value>
        public TServerService Server { get; private set; }

        /// <summary>
        ///     Called when the Start method of the ModSystem is called.
        /// </summary>
        /// <param name="api">The API.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public override void OnStart(ICoreAPI api)
        {
            // Start the core api.
            base.OnStart(api);

            // Instantiate the client and server services. 
            Client = Activator.CreateInstance<TClientService>();
            Server = Activator.CreateInstance<TServerService>();

            // Start the client and server services, as needed.
            switch (api.Side)
            {
                case EnumAppSide.Client:
                    Client.OnStart(api as ICoreClientAPI);
                    break;
                case EnumAppSide.Server:
                case EnumAppSide.Universal:
                    Server.OnStart(api as ICoreServerAPI);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(api.Side));
            }
        }
    }
}