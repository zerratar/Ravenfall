using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.ObjectActions
{
    public class TreeChopAction : SkillObjectAction
    {
        private readonly IKernel kernel;
        private readonly IRavenConnectionProvider connectionProvider;

        public TreeChopAction(
            IKernel kernel,
            IWorldProcessor worldProcessor,
            IItemProvider itemProvider,
            IObjectProvider objectProvider,
            IPlayerStatsProvider statsProvider,
            IPlayerInventoryProvider inventoryProvider,
            IRavenConnectionProvider connectionProvider)
            : base(1,
                  "Chop",
                  "Woodcutting",
                  2000,
                  worldProcessor,
                  itemProvider,
                  objectProvider,
                  statsProvider,
                  inventoryProvider)
        {
            this.kernel = kernel;
            this.connectionProvider = connectionProvider;
            AfterAction += (sender, ev) => MakeTreeStump(ev.Object);
        }

        private void MakeTreeStump(SceneObject tree)
        {
            tree.DisplayObjectId = 1;
            foreach (var playerConnection in connectionProvider.GetAll())
            {
                playerConnection.Send(ObjectUpdate.Create(tree), SendOption.Reliable);
            }

            kernel.SetTimeout(() => RespawnTree(tree), tree.RespawnMilliseconds);
        }

        private void RespawnTree(SceneObject tree)
        {            
            tree.DisplayObjectId = 0;
            foreach (var playerConnection in connectionProvider.GetAll())
            {
                playerConnection.Send(ObjectUpdate.Create(tree), SendOption.Reliable);
            }
        }
    }
}