using GameServer.Managers;
using GameServer.Network;
using GameServer.Processors;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;

public class TreeChopAction : SkillObjectAction
{
    private readonly IKernel kernel;

    public TreeChopAction(
        IKernel kernel,
        IWorldProcessor worldProcessor,
        IGameSessionManager sessionManager,
        IItemManager itemProvider,
        IPlayerStatsProvider statsProvider,
        IPlayerInventoryProvider inventoryProvider,
        IPlayerConnectionProvider connectionProvider)
        : base(1,
              "Chop",
              "Woodcutting",
              2000,
              worldProcessor,
              sessionManager,
              itemProvider,
              statsProvider,
              inventoryProvider)
    {
        this.kernel = kernel;
        AfterAction += (_, ev) => MakeTreeStump(ev.Object);
    }

    private void MakeTreeStump(WorldObject tree)
    {
        tree.DisplayObjectId = 0;
        World.UpdateObject(tree);
        kernel.SetTimeout(() => RespawnTree(tree), tree.RespawnMilliseconds);
    }

    private void RespawnTree(WorldObject tree)
    {        
        tree.DisplayObjectId = tree.ObjectId;
        World.UpdateObject(tree);
    }
}
