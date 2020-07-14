using GameServer.Managers;
using GameServer.Network;
using GameServer.Processors;
using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

public class TreeChopAction : SkillObjectAction
{
    private readonly IKernel kernel;
    private readonly IPlayerConnectionProvider connectionProvider;

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
        this.connectionProvider = connectionProvider;
        AfterAction += (sender, ev) => MakeTreeStump(ev.Object);
    }

    private void MakeTreeStump(WorldObject tree)
    {
        tree.DisplayObjectId = 0;
        foreach (var playerConnection in connectionProvider.GetAll())
        {
            playerConnection.Send(ObjectUpdate.Create(tree), SendOption.Reliable);
        }

        kernel.SetTimeout(() => RespawnTree(tree), tree.RespawnMilliseconds);
    }

    private void RespawnTree(WorldObject tree)
    {
        tree.DisplayObjectId = tree.ObjectId;
        foreach (var playerConnection in connectionProvider.GetAll())
        {
            playerConnection.Send(ObjectUpdate.Create(tree), SendOption.Reliable);
        }
    }
}
