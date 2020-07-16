using GameServer.Managers;
using GameServer.Processors;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;

public abstract class SkillObjectAction : EntityAction
{
    private readonly string skillName;
    private readonly int actionTime; 
    private readonly IPlayerStatsProvider statsProvider;
    private readonly IPlayerInventoryProvider inventoryProvider;
    private readonly IItemManager itemProvider;
    private readonly Random random = new Random();


    protected readonly IWorldProcessor World;
    protected readonly IGameSessionManager Sessions;
    protected event EventHandler<AfterActionEventArgs> AfterAction;

    protected SkillObjectAction(
        int id,
        string name,
        string skillName,
        int actionTime,
        IWorldProcessor worldProcessor,
        IGameSessionManager gameSessionManager,
        IItemManager itemProvider,
        IPlayerStatsProvider statsProvider,
        IPlayerInventoryProvider inventoryProvider)
        : base(id, name)
    {
        this.skillName = skillName;
        this.actionTime = actionTime;
        this.World = worldProcessor;
        this.Sessions = gameSessionManager;
        this.itemProvider = itemProvider;
        this.statsProvider = statsProvider;
        this.inventoryProvider = inventoryProvider;
    }

    public override bool Invoke(Player player, Entity entity, int parameterId)
    {
        if (!(entity is WorldObject obj))
        {
            return false;
        }

        var session = Sessions.Get(player);

        // if we are already interacting with this object
        // ignore it.
        if (session.Objects.HasAcquiredLock(obj, player))
        {
            return false;
        }

        if (!session.Objects.AcquireLock(obj, player))
        {
            return false;
        }

        if (obj.InteractItemType > 0)
        {
            var inventory = inventoryProvider.GetInventory(player.Id);
            var requiredItem = inventory.GetItemOfType(obj.InteractItemType);
            if (requiredItem == null || requiredItem.Amount < 1)
            {
                return false;
            }

            if (requiredItem.Item.Equippable)
            {
                World.SetItemEquipState(player, requiredItem.Item, true);
            }
        }

        StartAnimation(player, obj);
        return true;
    }

    protected bool HandleObjectTick(Player player, WorldObject obj, TimeSpan totalTime, TimeSpan deltaTime)
    {
        var session = Sessions.Get(player);
        if (!session.Objects.HasAcquiredLock(obj, player))
        {
            StopAnimation(player, obj);
            return true;
        }

        var skill = statsProvider.GetStatByName(player.Id, skillName);
        var chance = 0.05f + skill.EffectiveLevel * 0.05f;
        if (random.NextDouble() <= chance)
        {
            StartAnimation(player, obj);
            return false;
        }

        var levelsGaiend = skill.AddExperience(obj.Experience);
        var itemDrops = session.Objects.GetItemDrops(obj);

        foreach (var itemDrop in itemDrops)
        {
            if (random.NextDouble() > itemDrop.DropChance)
                continue;

            World.AddPlayerItem(player, itemProvider.GetItemById(itemDrop.ItemId));
        }

        World.UpdatePlayerStat(player, skill);

        if (levelsGaiend > 0)
        {
            World.PlayerStatLevelUp(player, skill, levelsGaiend);
        }

        StopAnimation(player, obj);
        if (AfterAction != null)
        {
            AfterAction?.Invoke(this, new AfterActionEventArgs(player, obj));
        }
        return true;
    }

    protected void StartAnimation(Player player, WorldObject obj)
    {
        World.PlayAnimation(player, skillName, true, true);
        World.SetEntityTimeout(actionTime, player, obj, HandleObjectTick);
    }

    protected void StopAnimation(Player player, WorldObject obj)
    {
        var session = Sessions.Get(player);
        var inventory = inventoryProvider.GetInventory(player.Id);
        var requiredItem = inventory.GetItemOfType(obj.InteractItemType);
        if (requiredItem != null)
        {
            if (requiredItem.Item.Equippable)
            {
                World.SetItemEquipState(player, requiredItem.Item, false);
            }
            if (requiredItem.Item.Consumable)
            {
                World.RemovePlayerItem(player, requiredItem.Item);
            }
            World.PlayAnimation(player, skillName, false);
        }

        session.Objects.ReleaseLocks(player);
    }
}
