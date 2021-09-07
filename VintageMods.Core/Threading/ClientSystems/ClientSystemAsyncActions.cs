using System.Collections.Concurrent;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace VintageMods.Core.Threading.ClientSystems
{
    public class ClientSystemAsyncActions : ClientSystem
    {
        private static ConcurrentQueue<Action> AsyncActions { get; set; } = new();
        private static ConcurrentQueue<Action> MainThreadActions { get; set; } = new();

        private readonly ClientMain _game;

        public ClientSystemAsyncActions(ClientMain game) : base(game) { _game = game; }

        public override string Name => "AsyncActions";

        public override EnumClientSystemType GetSystemType() => EnumClientSystemType.Misc;

        public override void OnSeperateThreadGameTick(float dt)
        {
            ProcessActions(AsyncActions);
            ProcessMainThreadActions();
        }

        private void ProcessMainThreadActions()
        {
            if (!MainThreadActions.IsEmpty)
                _game.EnqueueMainThreadTask(() => ProcessActions(MainThreadActions), "");
        }
        
        private static void ProcessActions(ConcurrentQueue<Action> actions)
        {
            for (var i = 0; i < actions.Count; i++)
            {
                var success = actions.TryDequeue(out var action);
                if (success) action.Invoke();
            }
        }

        public void Dispose(ICoreClientAPI capi) => Dispose(capi.World as ClientMain);

        public override void Dispose(ClientMain game)
        {
            if (!AsyncActions.IsEmpty)
                AsyncActions = new ConcurrentQueue<Action>();

            if (!MainThreadActions.IsEmpty)
                MainThreadActions = new ConcurrentQueue<Action>();

            base.Dispose(game);
        }

        public static void EnqueueAsyncTask(Action action)
        {
            AsyncActions.Enqueue(action);
        }

        public static void EnqueueMainThreadTask(Action action)
        {
            MainThreadActions.Enqueue(action);
        }
    }
}
