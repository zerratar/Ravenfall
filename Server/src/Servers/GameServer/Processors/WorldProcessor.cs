using System;
using System.Collections.Generic;
using System.Linq;
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
    private readonly IObjectProvider objectProvider;

    private readonly object mutex = new object();
    private readonly List<ObjectTick> objectTicks = new List<ObjectTick>();

    public WorldProcessor(
        ILogger logger,
        IKernel kernel,
        IPlayerStatsProvider statsProvider,
        IRavenConnectionProvider connectionProvider,
        IPlayerProvider playerProvider,
        IObjectProvider objectProvider)
    {
        this.logger = logger;
        this.kernel = kernel;
        this.statsProvider = statsProvider;
        this.connectionProvider = connectionProvider;
        this.playerProvider = playerProvider;
        this.objectProvider = objectProvider;
        this.kernel.RegisterTickUpdate(Update, TimeSpan.FromSeconds(60f / 1f));
    }

    public void AddPlayer(PlayerConnection myConnection)
    {
        var connections = connectionProvider.GetAll().OfType<PlayerConnection>();
        var allPlayers = playerProvider.GetAll();
        var objects = objectProvider.GetAll();

        foreach (var connection in connections)
        {
            var isMe = connection.InstanceID == myConnection.InstanceID;
            if (isMe)
            {
                var stats = statsProvider.GetStats(myConnection.Player.Id);
                connection.Send(MyPlayerAdd.Create(myConnection.Player, stats), SendOption.Reliable);
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
            myConnection.Send(ObjectAdd.Create(obj), SendOption.Reliable);
        }
    }

    public void RemovePlayer(Player player)
    {
        objectProvider.ReleaseObjectLocks(player);

        var connections = connectionProvider.GetConnected();
        foreach (var connection in connections)
        {
            connection.Send(PlayerRemove.Create(player), SendOption.Reliable);
        }
    }

    public void PlayAnimation(Player player, string animation, bool enabled = true, bool trigger = false, int number = 0)
    {
        var connections = connectionProvider.GetConnected();
        foreach (var connection in connections)
        {
            connection.Send(PlayerAnimationStateUpdate.Create(player, animation, enabled, trigger, number), SendOption.Reliable);
        }
    }

    public void UpdatePlayerStat(Player player, PlayerStat skill)
    {
        var playerConnection = connectionProvider
            .GetAll()
            .OfType<PlayerConnection>()
            .FirstOrDefault(x => x != null && x.Player.Id == player.Id);

        if (playerConnection != null)
        {
            playerConnection.Send(PlayerStatUpdate.Create(player, skill), SendOption.Reliable);
        }
    }

    public void PlayerStatLevelUp(Player player, PlayerStat skill, int levelsGained)
    {
        var connections = connectionProvider.GetConnected();
        foreach (var connection in connections)
        {
            connection.Send(PlayerLevelUp.Create(player, skill, levelsGained), SendOption.Reliable);
        }
    }

    public void SetItemEquipState(Player player, Item item, bool state)
    {
        var connections = connectionProvider.GetConnected();
        foreach (var connection in connections)
        {
            connection.Send(PlayerEquipmentStateUpdate.Create(player, item, state), SendOption.Reliable);
        }
    }

    public void PlayerObjectInteraction(
        Player player, SceneObject obj, SceneObjectAction action, int parameterId)
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

    public void RegisterObjectTickUpdate<TObject>(
        Player player, TObject obj, ObjectTickHandler<TObject> handleObjectTick)
        where TObject : SceneObject
    {
        lock (mutex) objectTicks.Add(new ObjectTick<TObject>(player, obj, handleObjectTick));
    }
    public ITimeoutHandle SetObjectTimeout<TObject>(int milliseconds, Player player, TObject obj, ObjectTickHandler<TObject> handleObjectTick) where TObject : SceneObject
    {
        var tick = new ObjectTick<TObject>(player, obj, handleObjectTick);
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

    private void Update(TimeSpan deltaTime)
    {
        // Server Tick

        lock (mutex)
        {
            var ticksToRemove = new List<ObjectTick>();
            foreach (var objectTick in objectTicks)
            {
                if (objectTick.Invoke(deltaTime))
                {
                    ticksToRemove.Add(objectTick);
                }
            }

            foreach (var remove in ticksToRemove)
            {
                objectTicks.Remove(remove);
            }
        }
    }

}