using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RavenfallServer.Packets;
using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

public class WorldProcessor : IWorldProcessor
{
    private readonly ILogger logger;
    private readonly IKernel kernel;
    private readonly IPlayerStatsProvider statsProvider;
    private readonly IRavenConnectionProvider connectionProvider;
    private readonly IPlayerProvider playerProvider;
    private readonly IPlayerInventoryProvider playerInventoryProvider;
    private readonly INpcShopInventoryProvider shopInventoryProvider;
    private readonly IObjectProvider objectProvider;
    private readonly INpcProvider npcProvider;
    private readonly IItemProvider itemProvider;

    private readonly object objectUpdateMutex = new object();
    private readonly object playerMutex = new object();

    private readonly List<EntityTick> entityIntervalItems = new List<EntityTick>();
    private readonly List<Player> activePlayers = new List<Player>();

    public WorldProcessor(
        ILogger logger,
        IKernel kernel,
        IPlayerStatsProvider statsProvider,
        IPlayerInventoryProvider playerInventoryProvider,
        INpcShopInventoryProvider shopInventoryProvider,
        IRavenConnectionProvider connectionProvider,
        IPlayerProvider playerProvider,
        IObjectProvider objectProvider,
        INpcProvider npcProvider,
        IItemProvider itemProvider)
    {
        this.logger = logger;
        this.kernel = kernel;
        this.statsProvider = statsProvider;
        this.playerInventoryProvider = playerInventoryProvider;
        this.shopInventoryProvider = shopInventoryProvider;
        this.connectionProvider = connectionProvider;
        this.playerProvider = playerProvider;
        this.objectProvider = objectProvider;
        this.npcProvider = npcProvider;
        this.itemProvider = itemProvider;
        this.kernel.RegisterTickUpdate(Update, TimeSpan.FromSeconds(60f / 1f));
    }

    public void SendChatMessage(Player player, int channelID, string message)
    {
        var connections = GetConnectedActivePlayerConnections();

        // var connections = chatHandler.GetChannelPlayers(channelID);

        foreach (var connection in connections)
        {
            connection.Send(ChatMessage.Create(player, channelID, message), SendOption.Reliable);
        }
    }

    public void AddPlayer(PlayerConnection myConnection)
    {
        try
        {
            AddActivePlayer(myConnection.Player);
            var connections = GetConnectedActivePlayerConnections();
            var allPlayers = GetActivePlayers();
            var objects = objectProvider.GetAll();
            var npcs = npcProvider.GetAll();

            foreach (var connection in connections)
            {
                var isMe = connection.InstanceID == myConnection.InstanceID;
                if (isMe)
                {
                    var stats = statsProvider.GetStats(myConnection.Player.Id);
                    var level = statsProvider.GetCombatLevel(myConnection.Player.Id);
                    var inventory = playerInventoryProvider.GetInventory(myConnection.Player.Id);
                    connection.Send(MyPlayerAdd.Create(myConnection.Player, level, stats, inventory.Items), SendOption.Reliable);
                    //connection.Send(PlayerInventory.Create(myConnection.Player, inventory.Items), SendOption.Reliable);
                }
                else
                {
                    var combatLevel = statsProvider.GetCombatLevel(connection.Player.Id);
                    connection.Send(PlayerAdd.Create(myConnection.Player, combatLevel), SendOption.Reliable);
                }
            }

            foreach (var player in allPlayers)
            {
                if (player.Id == myConnection.Player.Id) continue;
                var combatLevel = statsProvider.GetCombatLevel(myConnection.Player.Id);
                myConnection.Send(PlayerAdd.Create(player, combatLevel), SendOption.Reliable);
            }

            foreach (var obj in objects)
            {
                if (obj.Static)
                {
                    if (obj.DisplayObjectId != obj.ObjectId)
                    {
                        myConnection.Send(ObjectUpdate.Create(obj), SendOption.Reliable);
                    }
                }
                else
                {
                    myConnection.Send(ObjectAdd.Create(obj), SendOption.Reliable);
                }
            }


            foreach (var npc in npcs)
            {
                myConnection.Send(NpcAdd.Create(npc), SendOption.Reliable);
            }
        }
        catch (Exception exc)
        {
            logger.Error(exc.ToString());
        }
    }

    public void RemovePlayer(Player player)
    {
        RemoveActivePlayer(player);
        objectProvider.ReleaseLocks(player);

        foreach (var connection in GetConnectedActivePlayerConnections())
        {
            connection.Send(PlayerRemove.Create(player), SendOption.Reliable);
        }
    }

    public void PlayAnimation(Player player, string animation, bool enabled = true, bool trigger = false, int number = 0)
    {
        foreach (var connection in GetConnectedActivePlayerConnections())
        {
            connection.Send(PlayerAnimationStateUpdate.Create(player, animation, enabled, trigger, number), SendOption.Reliable);
        }
    }

    public void PlayAnimation(Npc npc, string animation, bool enabled = true, bool trigger = false, int number = 0)
    {
        foreach (var connection in GetConnectedActivePlayerConnections())
        {
            connection.Send(NpcAnimationStateUpdate.Create(npc, animation, enabled, trigger, number), SendOption.Reliable);
        }
    }

    public void UpdatePlayerStat(Player player, EntityStat skill)
    {
        var playerConnection = GetPlayerConnection(player);
        if (playerConnection != null)
        {
            playerConnection.Send(PlayerStatUpdate.Create(player, skill), SendOption.Reliable);
        }
    }
    public void AddPlayerItem(Player player, Item item, int amount = 1)
    {
        var playerConnection = GetPlayerConnection(player);
        var inventory = playerInventoryProvider.GetInventory(player.Id);
        if (inventory != null)
        {
            inventory.AddItem(item, amount);
        }

        if (playerConnection != null)
        {
            playerConnection.Send(PlayerItemAdd.Create(player, item, amount), SendOption.Reliable);
        }
    }
    public void RemovePlayerItem(Player player, Item item, int amount = 1)
    {
        var playerConnection = GetPlayerConnection(player);
        var inventory = playerInventoryProvider.GetInventory(player.Id);
        if (inventory != null)
        {
            inventory.RemoveItem(item, amount);
        }

        if (playerConnection != null)
        {
            playerConnection.Send(PlayerItemRemove.Create(player, item, amount), SendOption.Reliable);
        }
    }


    public void PlayerBuyItem(Player player, Npc npc, int itemId, int amount)
    {
        var item = itemProvider.GetItemById(itemId);
        if (item == null) return;

        var shopInventory = shopInventoryProvider.GetInventory(npc.Id);
        if (shopInventory == null) return;

        var shopItem = shopInventory.GetItem(itemId);

        var totalPrice = shopItem.Price * amount;
        if (player.Coins < totalPrice) return;

        if (shopInventory.TryRemoveItem(item, amount))
        {
            player.Coins -= totalPrice;

            UpdatePlayerCoins(player);
            AddPlayerItem(player, item, amount);
            NpcTradeUpdateStock(npc);
        }
    }

    public void PlayerSellItem(Player player, Npc npc, int itemId, int amount)
    {
        var item = itemProvider.GetItemById(itemId);
        if (item == null) return;

        var inventory = playerInventoryProvider.GetInventory(player.Id);
        if (inventory == null) return;

        var shopInventory = shopInventoryProvider.GetInventory(npc.Id);
        if (shopInventory == null) return;

        if (inventory.HasItem(item, amount))
        {
            var shopItem = shopInventory.GetItem(item.Id);
            if (shopItem == null) return;

            //player.Coins += shopItem.Item.Value * amount;
            player.Coins += shopItem.Price * amount;

            RemovePlayerItem(player, item, amount);
            shopInventory.AddItem(item, amount, shopItem.Price);
            NpcTradeUpdateStock(npc);
            UpdatePlayerCoins(player);
        }
    }

    public void UpdatePlayerCoins(Player player)
    {
        var connection = GetPlayerConnection(player);
        if (connection == null) return;

        connection.Send(new PlayerCoinsUpdate
        {
            Coins = player.Coins,
            PlayerId = connection.Player.Id
        }, SendOption.Reliable);
    }

    public void NpcTradeUpdateStock(Npc npc)
    {
        var shopInventory = shopInventoryProvider.GetInventory(npc.Id);
        if (shopInventory == null) return;

        var itemCount = shopInventory.Items.Count;
        var itemIds = new int[itemCount];
        var itemPrice = new int[itemCount];
        var itemStock = new int[itemCount];

        for (var i = 0; i < itemCount; ++i)
        {
            var item = shopInventory.Items[i];
            itemIds[i] = item.Item.Id;
            itemPrice[i] = item.Price;
            itemStock[i] = item.Amount;
        }

        foreach (var connection in GetConnectedActivePlayerConnections())
        {
            connection.Send(new NpcTradeUpdateStock
            {
                ItemId = itemIds,
                ItemPrice = itemPrice,
                ItemStock = itemStock,
                NpcServerId = npc.Id,
                PlayerId = connection.Player.Id
            }, SendOption.Reliable);
        }
    }

    public void PlayerStatLevelUp(Player player, EntityStat skill, int levelsGained)
    {
        foreach (var connection in GetConnectedActivePlayerConnections())
        {
            connection.Send(PlayerLevelUp.Create(player, skill, levelsGained), SendOption.Reliable);
        }
    }

    public void NpcDamage(Player player, Npc npc, int damage, int health, int maxHealth)
    {
        foreach (var connection in GetConnectedActivePlayerConnections())
        {
            connection.Send(NpcHealthChange.Create(npc, player, -damage, health, maxHealth), SendOption.Reliable);
        }
    }

    public void NpcDeath(Player player, Npc npc)
    {
        foreach (var connection in GetConnectedActivePlayerConnections())
        {
            connection.Send(RavenfallServer.Packets.NpcDeath.Create(npc, player), SendOption.Reliable);
        }
    }

    public void NpcRespawn(Player player, Npc npc)
    {
        foreach (var connection in GetConnectedActivePlayerConnections())
        {
            connection.Send(RavenfallServer.Packets.NpcRespawn.Create(npc, player), SendOption.Reliable);
        }
    }

    public void SetItemEquipState(Player player, Item item, bool state)
    {
        var inventory = playerInventoryProvider.GetInventory(player.Id);
        if (state)
            inventory.EquipItem(item);
        else
            inventory.UnEquipItem(item);

        foreach (var connection in GetConnectedActivePlayerConnections())
        {
            connection.Send(PlayerEquipmentStateUpdate.Create(player, item, state), SendOption.Reliable);
        }
    }

    public void OpenTradeWindow(Player player, Npc npc, string shopName, ShopInventory shopInventory)
    {
        var playerConnection = GetPlayerConnection(player);
        if (playerConnection == null)
        {
            return;
        }

        var itemCount = shopInventory.Items.Count;
        var itemIds = new int[itemCount];
        var itemPrice = new int[itemCount];
        var itemStock = new int[itemCount];

        for (var i = 0; i < itemCount; ++i)
        {
            var item = shopInventory.Items[i];
            itemIds[i] = item.Item.Id;
            itemPrice[i] = item.Price;
            itemStock[i] = item.Amount;
        }

        playerConnection.Send(new NpcTradeOpenWindow
        {
            PlayerId = player.Id,
            ItemId = itemIds,
            ItemPrice = itemPrice,
            ItemStock = itemStock,
            ShopName = shopName,
            NpcServerId = npc.Id,
        }, SendOption.Reliable);
    }

    public void PlayerNpcInteraction(
        Player player, Npc npc, EntityAction action, int parameterId)
    {
        if (action.Invoke(player, npc, parameterId))
        {
            foreach (var playerConnection in connectionProvider.GetAll())
            {
                playerConnection.Send(new PlayerNpcActionResponse
                {
                    PlayerId = player.Id,
                    ActionId = action.Id,
                    NpcServerId = npc.Id,
                    ParameterId = parameterId,
                    Status = 1 // 0: fail, 1: success
                }, SendOption.Reliable);
            }
        }
    }

    public void PlayerObjectInteraction(
        Player player, SceneObject obj, EntityAction action, int parameterId)
    {
        // use 
        if (action.Invoke(player, obj, parameterId))
        {
            foreach (var playerConnection in connectionProvider.GetAll())
            {
                playerConnection.Send(new PlayerObjectActionResponse
                {
                    PlayerId = player.Id,
                    ActionId = action.Id,
                    ObjectServerId = obj.Id,
                    ParameterId = parameterId,
                    Status = 1 // 0: fail, 1: success
                }, SendOption.Reliable);
            }
        }
    }

    public void SetEntityInterval<TObject>(
        Player player, TObject obj, EntityTickHandler<TObject> handleObjectTick)
        where TObject : Entity
    {
        lock (objectUpdateMutex)
            entityIntervalItems.Add(
                new EntityTick<TObject>(player, obj, handleObjectTick));
    }
    public ITimeoutHandle SetEntityTimeout<TObject>(
        int milliseconds,
        Player player,
        TObject obj,
        EntityTickHandler<TObject> handleObjectTick)
        where TObject : Entity
    {
        var tick = new EntityTick<TObject>(player, obj, handleObjectTick);
        var started = DateTime.Now;
        return kernel.SetTimeout(() =>
        {
            var elapsed = DateTime.Now - started;
            tick.Invoke(elapsed);
        }, milliseconds);
    }

    public void ClearObjectTimeout(ITimeoutHandle handle)
    {
        kernel.ClearTimeout(handle);
    }

    private IReadOnlyList<Player> GetActivePlayers()
    {
        lock (playerMutex) return activePlayers;
    }

    private void AddActivePlayer(Player player)
    {
        lock (playerMutex) activePlayers.Add(player);
    }

    private void RemoveActivePlayer(Player player)
    {
        lock (playerMutex) activePlayers.Remove(player);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private PlayerConnection GetPlayerConnection(Player player)
    {
        return connectionProvider
            .GetConnection<PlayerConnection>(x =>
               x != null &&
               x.Player.Id == player.Id);
    }

    /// <summary>
    /// Gets all active players regardless of their connection state.
    /// </summary>
    /// <remarks>Use this when you need to access all players regardless of their current state of connection. 
    /// This can be helpful in case some players get a temporary disconnection due to server lag.</remarks>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerable<PlayerConnection> GetAllActivePlayerConnections()
    {
        return connectionProvider
            .GetAll()
            .OfType<PlayerConnection>()
            .Where(x => x.Player != null);
    }

    /// <summary>
    /// Gets all active players with a known connected state
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private IEnumerable<PlayerConnection> GetConnectedActivePlayerConnections()
    {
        return connectionProvider
            .GetConnected()
            .OfType<PlayerConnection>()
            .Where(x => x.Player != null);
    }

    private void Update(TimeSpan deltaTime)
    {
        // Server Tick

        lock (objectUpdateMutex)
        {
            var ticksToRemove = new List<EntityTick>();
            foreach (var objectTick in entityIntervalItems)
            {
                if (objectTick.Invoke(deltaTime))
                {
                    ticksToRemove.Add(objectTick);
                }
            }

            foreach (var remove in ticksToRemove)
            {
                entityIntervalItems.Remove(remove);
            }
        }
    }
}