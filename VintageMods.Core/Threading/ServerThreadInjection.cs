using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VintageMods.Core.Reflection;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.Server;

namespace VintageMods.Core.Threading
{
    public static class ServerThreadInjection
    {
        private static readonly Type ServerThread;

        static ServerThreadInjection()
        {
            ServerThread = typeof(ServerMain).Assembly.GetClassType("ServerThread");
        }

        public static List<Thread> GetServerThreads(this IServerWorldAccessor world)
        {
            return (world as ServerMain).GetField<List<Thread>>("Serverthreads");
        }

        public static Stack<ServerSystem> GetVanillaSystems(this IServerWorldAccessor world)
        {
            return new((world as ServerMain).GetField<ServerSystem[]>("Systems"));
        }

        public static void InjectServerThread(this ICoreServerAPI sapi, string name,
            params ServerSystem[] systems)
        {
            sapi.World.InjectServerThread(name, systems);
        }


        private static object CreateServerThread(IServerWorldAccessor world, string name,
            IEnumerable<ServerSystem> systems)
        {
            var instance = ServerThread.CreateInstance();
            instance.SetField("server", world as ServerMain);
            instance.SetField("threadName", name);
            instance.SetField("serversystems", systems.ToArray());
            instance.SetField("lastFramePassedTime", new Stopwatch());
            instance.SetField("totalPassedTime", new Stopwatch());
            instance.SetField("paused", false);
            instance.SetField("alive", true);
            return instance;
        }

        public static void InjectServerThread(this IServerWorldAccessor world, string name,
            params ServerSystem[] systems)
        {
            var instance = CreateServerThread(world, name, systems);
            var serverThreads = world.GetServerThreads();
            var vanillaSystems = world.GetVanillaSystems();

            foreach (var system in systems) vanillaSystems.Push(system);

            (world as ServerMain).SetField("Systems", vanillaSystems.ToArray());

            var thread = new Thread(() => instance.CallMethod("Process")) { IsBackground = true };
            thread.Start();
            thread.Name = name;
            serverThreads.Add(thread);
        }
    }
}