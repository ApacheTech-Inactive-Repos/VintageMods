using System.Collections.Concurrent;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace VintageMods.Core.Threading.Systems
{
    public class ClientSystemAsyncActions : ClientSystem
    {
        private readonly ClientMain _game;

        public ClientSystemAsyncActions(ClientMain game) : base(game)
        {
            _game = game;
        }

        private ConcurrentQueue<Action> AsyncActions { get; set; } = new();
        private ConcurrentQueue<Action> MainThreadActions { get; set; } = new();

        public override string Name => "AsyncActions";

        public override EnumClientSystemType GetSystemType()
        {
            return EnumClientSystemType.Misc;
        }

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

        public void Dispose(ICoreClientAPI capi)
        {
            Dispose(capi.World as ClientMain);
        }

        public override void Dispose(ClientMain game)
        {
            if (!AsyncActions.IsEmpty)
                AsyncActions = new ConcurrentQueue<Action>();

            if (!MainThreadActions.IsEmpty)
                MainThreadActions = new ConcurrentQueue<Action>();

            base.Dispose(game);
        }

        public void EnqueueAsyncTask(Action action)
        {
            AsyncActions.Enqueue(action);
        }

        public void EnqueueMainThreadTask(Action action)
        {
            MainThreadActions.Enqueue(action);
        }
    }
}