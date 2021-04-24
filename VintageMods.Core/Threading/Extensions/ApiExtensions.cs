using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

namespace VintageMods.Core.Threading.Extensions
{
    public static class ApiExtensions
    {
        public static void EnableAsyncTasks(this ICoreClientAPI api)
        {
            api.Event.LevelFinalize += () =>
            {
                if (!api.IsClientSystemLoaded<VintageModsGameThread>())
                    api.InjectClientThread("VintageModsAsync", 20, new VintageModsGameThread(api.World as ClientMain));
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
    }
}
