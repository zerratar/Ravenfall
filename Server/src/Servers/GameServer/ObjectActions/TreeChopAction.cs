using RavenfallServer.Objects;
using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;
using System;

public class TreeChopAction : SceneObjectAction
{
    private const int actionTime = 2000;
    private const string SkillName = "Woodcutting";

    private readonly IKernel kernel;
    private readonly IWorldProcessor worldProcessor;
    private readonly IObjectProvider objectProvider;
    private readonly IPlayerStatsProvider statsProvider;
    private readonly IItemProvider itemProvider;
    private readonly IRavenConnectionProvider connectionProvider;
    private readonly Random random = new Random();

    public TreeChopAction(
        IKernel kernel,
        IWorldProcessor worldProcessor,
        IItemProvider itemProvider,
        IObjectProvider objectProvider,
        IPlayerStatsProvider statsProvider,
        IRavenConnectionProvider connectionProvider)
        : base(1, "Chop")
    {
        this.kernel = kernel;
        this.worldProcessor = worldProcessor;
        this.itemProvider = itemProvider;
        this.objectProvider = objectProvider;
        this.statsProvider = statsProvider;
        this.connectionProvider = connectionProvider;
    }

    public override bool Invoke(Player player, SceneObject obj, int parameterId)
    {
        if (!(obj is TreeObject tree))
        {
            return false;
        }

        if (tree.ObjectId == 1)
        {
            return false;
        }

        if (!objectProvider.AcquireObjectLock(tree, player))
        {
            // another player is currently having a blast with this tree
            return false;
        }

        var item = itemProvider.GetItemById(0);
        if (item != null)
        {
            worldProcessor.SetItemEquipState(player, item, true);
        }

        StartWoodcuttingAnimation(player, tree);

        //CutDownTree(player, tree);

        return true;
    }

    private bool HandleObjectTick(Player player, TreeObject tree, TimeSpan totalTime, TimeSpan deltaTime)
    {
        if (!objectProvider.HasAcquiredObjectLock(tree, player))
        {
            StopWoodcuttingAnimation(player);
            return true;
        }

        var skill = statsProvider.GetStatByName(player.Id, SkillName);
        var chance = 0.05f + (skill.EffectiveLevel * 0.05f);
        if (random.NextDouble() <= chance)
        {
            StartWoodcuttingAnimation(player, tree);
            return false;
        }

        var levelsGaiend = skill.AddExperience(tree.Experience);

        worldProcessor.UpdatePlayerStat(player, skill);

        if (levelsGaiend > 0)
        {
            worldProcessor.PlayerStatLevelUp(player, skill, levelsGaiend);
        }

        StopWoodcuttingAnimation(player);
        MakeTreeStump(tree);
        return true;
    }

    private void StartWoodcuttingAnimation(Player player, TreeObject tree)
    {
        worldProcessor.PlayAnimation(player, SkillName, true, true);
        worldProcessor.SetObjectTimeout(actionTime, player, tree, HandleObjectTick);
    }

    private void StopWoodcuttingAnimation(Player player)
    {
        var item = itemProvider.GetItemById(0);
        if (item != null)
        {
            worldProcessor.SetItemEquipState(player, item, false);
        }

        worldProcessor.PlayAnimation(player, SkillName, false);
        objectProvider.ReleaseObjectLocks(player);
    }

    private void MakeTreeStump(TreeObject tree)
    {
        tree.ObjectId = 1;
        foreach (var playerConnection in connectionProvider.GetAll())
        {
            playerConnection.Send(ObjectUpdate.Create(tree), SendOption.Reliable);
        }

        kernel.SetTimeout(() => RespawnTree(tree), tree.RespawnMilliseconds);
    }

    private void RespawnTree(TreeObject tree)
    {
        tree.ObjectId = 0;
        foreach (var playerConnection in connectionProvider.GetAll())
        {
            playerConnection.Send(ObjectUpdate.Create(tree), SendOption.Reliable);
        }
    }
}