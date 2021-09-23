using System.Linq;
using VintageMods.Core.Extensions;
using VintageMods.Core.Threading.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

namespace VintageMods.Core.Threading.Extensions
{
    public static class ApiExtensions
    {
        public static void EnableAsyncTasks(this ICoreClientAPI api)
        {
            api.Event.LevelFinalize += () =>
            {
                if (!api.IsClientSystemLoaded<ClientSystemAsyncActions>())
                    api.InjectClientThread("AsyncActions", new ClientSystemAsyncActions(api.AsClientMain()));
            };
        }

        public static bool IsClientSystemLoaded(this ICoreClientAPI api, string name)
        {
            return api.World.GetVanillaSystems().Any(clientSystem => clientSystem.Name == name);
        }

        public static bool IsClientSystemLoaded<T>(this ICoreClientAPI api) where T : ClientSystem
        {
            return api.World.GetVanillaSystems().Any(clientSystem => clientSystem.GetType() == typeof(T));
        }

        public static void EnableAsyncTasks(this ICoreServerAPI api)
        {
            api.Event.SaveGameLoaded += () =>
            {
                if (!api.IsServerSystemLoaded<ServerSystemAsyncActions>())
                    api.InjectServerThread("AsyncActions", new ServerSystemAsyncActions(api.World as ServerMain));
            };
        }

        public static bool IsServerSystemLoaded<TServerSystem>(this ICoreServerAPI api)
            where TServerSystem : ServerSystem
        {
            return api.World.GetVanillaSystems().Any(clientSystem => clientSystem.GetType() == typeof(TServerSystem));
        }
    }
}