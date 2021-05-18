using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VintageMods.Core.Reflection;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

// ReSharper disable StringLiteralTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace VintageMods.Core.Threading
{
    public static class ClientThreadInjection
    {
        private static readonly Type ClientThread;

        static ClientThreadInjection()
        {
            ClientThread = typeof(ClientMain).Assembly.GetClassType("ClientThread");
        }

        public static List<Thread> GetClientThreads(this IClientWorldAccessor world)
        {
            return (world as ClientMain).GetField<List<Thread>>("clientThreads");
        }

        public static Stack<ClientSystem> GetVanillaSystems(this IClientWorldAccessor world)
        {
            return new Stack<ClientSystem>((world as ClientMain).GetField<ClientSystem[]>("clientSystems"));
        }

        public static void InjectClientThread(this ICoreClientAPI capi, string name, int ms,
            params ClientSystem[] systems)
        {
            capi.World.InjectClientThread(name, ms, systems);
        }


        private static object CreateClientThread(IClientWorldAccessor world, string name, int ms,
            IEnumerable<ClientSystem> systems)
        {
            var instance = ClientThread.CreateInstance();
            instance.SetField("game", world as ClientMain);
            instance.SetField("threadName", name);
            instance.SetField("clientsystems", systems.ToArray());
            instance.SetField("lastFramePassedTime", new Stopwatch());
            instance.SetField("totalPassedTime", new Stopwatch());
            instance.SetField("paused", false);
            instance.SetField("sleepMs", ms);
            return instance;
        }

        public static void InjectClientThread(this IClientWorldAccessor world, string name, int ms,
            params ClientSystem[] systems)
        {
            var instance = CreateClientThread(world, name, ms, systems);
            var clientThreads = world.GetClientThreads();
            var vanillaSystems = world.GetVanillaSystems();

            foreach (var system in systems) vanillaSystems.Push(system);

            (world as ClientMain).SetField("clientSystems", vanillaSystems.ToArray());

            var thread = new Thread(() => instance.CallMethod("Process")) {IsBackground = true};
            thread.Start();
            thread.Name = name;
            clientThreads.Add(thread);
        }
    }
}