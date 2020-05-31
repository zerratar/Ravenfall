using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;

public abstract class SkillObjectAction : SceneObjectAction
{
    private readonly string skillName;
    private readonly int actionTime;
    private readonly IWorldProcessor worldProcessor;
    private readonly IObjectProvider objectProvider;
    private readonly IPlayerStatsProvider statsProvider;
    private readonly IPlayerInventoryProvider inventoryProvider;
    private readonly IItemProvider itemProvider;
    private readonly Random random = new Random();

    public event EventHandler<AfterActionEventArgs> AfterAction;

    protected SkillObjectAction(
        int id,
        string name,
        string skillName,
        int actionTime,
        IWorldProcessor worldProcessor,
        IItemProvider itemProvider,
        IObjectProvider objectProvider,
        IPlayerStatsProvider statsProvider,
        IPlayerInventoryProvider inventoryProvider)
        : base(id, name)
    {
        this.skillName = skillName;
        this.actionTime = actionTime;
        this.worldProcessor = worldProcessor;
        this.itemProvider = itemProvider;
        this.objectProvider = objectProvider;
        this.statsProvider = statsProvider;
        this.inventoryProvider = inventoryProvider;
    }

    public override bool Invoke(Player player, SceneObject obj, int parameterId)
    {
        // if we are already interacting with this object
        // ignore it.
        if (objectProvider.HasAcquiredObjectLock(obj, player))
        {
            return false;
        }

        if (!objectProvider.AcquireObjectLock(obj, player))
        {
            return false;
        }

        if (obj.InteractItemType > 0)
        {
            var inventory = inventoryProvider.GetInventory(player.Id);
            var requiredItem = inventory.GetItemOfType(obj.InteractItemType);
            if (requiredItem == null)
            {
                return false;
            }

            if (requiredItem.Item.Equippable)
            {
                worldProcessor.SetItemEquipState(player, requiredItem.Item, true);
            }
        }

        StartAnimation(player, obj);
        return true;
    }

    protected bool HandleObjectTick(Player player, SceneObject obj, TimeSpan totalTime, TimeSpan deltaTime)
    {
        if (!objectProvider.HasAcquiredObjectLock(obj, player))
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
        var itemDrops = objectProvider.GetItemDrops(obj);

        foreach (var itemDrop in itemDrops)
        {
            if (random.NextDouble() > itemDrop.DropChance)
                continue;

            worldProcessor.AddPlayerItem(player, itemProvider.GetItemById(itemDrop.ItemId));
        }

        worldProcessor.UpdatePlayerStat(player, skill);

        if (levelsGaiend > 0)
        {
            worldProcessor.PlayerStatLevelUp(player, skill, levelsGaiend);
        }

        StopAnimation(player, obj);
        if (AfterAction != null)
        {
            AfterAction?.Invoke(this, new AfterActionEventArgs(player, obj));
        }
        return true;
    }

    protected void StartAnimation(Player player, SceneObject obj)
    {
        worldProcessor.PlayAnimation(player, skillName, true, true);
        worldProcessor.SetObjectTimeout(actionTime, player, obj, HandleObjectTick);
    }

    protected void StopAnimation(Player player, SceneObject obj)
    {
        var inventory = inventoryProvider.GetInventory(player.Id);
        var requiredItem = inventory.GetItemOfType(obj.InteractItemType);
        if (requiredItem != null)
        {
            if (requiredItem.Item.Equippable)
            {
                worldProcessor.SetItemEquipState(player, requiredItem.Item, false);
            }
            if (requiredItem.Item.Consumable)
            {
                worldProcessor.RemovePlayerItem(player, requiredItem.Item);
            }
            worldProcessor.PlayAnimation(player, skillName, false);
        }

        objectProvider.ReleaseObjectLocks(player);
    }
}
