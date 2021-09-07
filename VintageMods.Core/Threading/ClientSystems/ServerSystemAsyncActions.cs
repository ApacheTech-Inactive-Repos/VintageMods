using System.Collections.Concurrent;
using Vintagestory.API.Common;
using Vintagestory.Server;

namespace VintageMods.Core.Threading.ClientSystems
{
    public class ServerSystemAsyncActions : ServerSystem
    {
        private static ConcurrentQueue<Action> AsyncActions { get; set; } = new();
        private static ConcurrentQueue<Action> MainThreadActions { get; set; } = new();

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

        public static void EnqueueAsyncTask(Action action)
        {
            AsyncActions.Enqueue(action);
        }

        public static void EnqueueMainThreadTask(Action action)
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