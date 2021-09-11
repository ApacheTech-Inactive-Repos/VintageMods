using System.Collections.Concurrent;
using Vintagestory.API.Common;
using Vintagestory.Server;

namespace VintageMods.Core.Threading.Systems
{
    public class ServerSystemAsyncActions : ServerSystem
    {
        private ConcurrentQueue<Action> AsyncActions { get; set; } = new();
        private ConcurrentQueue<Action> MainThreadActions { get; set; } = new();

        private readonly ServerMain _game;

        public ServerSystemAsyncActions(ServerMain game) : base(game) { _game = game; }

        public override void OnSeperateThreadTick(float dt)
        {
            ProcessActions(AsyncActions);
            ProcessMainThreadActions();
        }

        private void ProcessMainThreadActions()
        {
            if (!MainThreadActions.IsEmpty)
                _game.EnqueueMainThreadTask(() => ProcessActions(MainThreadActions));
        }

        private static void ProcessActions(ConcurrentQueue<Action> actions)
        {
            for (var i = 0; i < actions.Count; i++)
            {
                var success = actions.TryDequeue(out var action);
                if (success) action.Invoke();
            }
        }

        public void EnqueueAsyncTask(Action action)
        {
            AsyncActions.Enqueue(action);
        }

        public void EnqueueMainThreadTask(Action action)
        {
            MainThreadActions.Enqueue(action);
        }

        public override void Dispose()
        {
            if (!AsyncActions.IsEmpty)
                AsyncActions = new ConcurrentQueue<Action>();

            if (!MainThreadActions.IsEmpty)
                MainThreadActions = new ConcurrentQueue<Action>();

            base.Dispose();
        }
    }
}