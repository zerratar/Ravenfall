using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Shinobytes.Ravenfall.RavenNet.Models;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private NametagManager nametagManager;
    [SerializeField] private HealthCounterManager healthCounterManager;

    private readonly List<NetworkPlayer> players = new List<NetworkPlayer>();

    private ServerPlayerAlignmentActions[] alignmentActions;
    public NetworkPlayer Me { get; private set; }
    
    private void Awake()
    {
        alignmentActions = Resources.LoadAll<ServerPlayerAlignmentActions>("Data/Players");
    }

    public NetworkPlayer GetPlayerById(int playerId)
    {
        return this.players.FirstOrDefault(x => x.Id == playerId);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal IReadOnlyList<ServerAction> GetPlayerAlignmentActions(PlayerAlignment alignment)
    {
        return alignmentActions.FirstOrDefault(x => x.Alignment == alignment).Actions.ToArray();
    }

    internal void OnPlayerMove(Player player)
    {
        var targetPlayer = players.FirstOrDefault(x => x.Id == player.Id);
        if (!targetPlayer)
        {
            return;
        }

        targetPlayer.Navigation.MoveTo(player.Position, player.Destination, player.Running);
    }

    public void OnPlayerAdded(Player player)
    {
        Debug.Log("OnPlayerAdded");
        var networkPlayerObject = Instantiate(playerPrefab);
        var networkPlayer = networkPlayerObject.GetComponent<NetworkPlayer>();
        networkPlayer.PlayerManager = this;
        networkPlayer.IsMe = player.IsMe;
        networkPlayer.Id = player.Id;
        networkPlayer.name = player.Name;

        var navMeshAgent = networkPlayer.GetComponent<NavMeshAgent>();
        if (navMeshAgent)
        {
            navMeshAgent.Warp(player.Position);
        }
        else
        {
            networkPlayer.transform.position = player.Position;
        }

        if (player.IsMe) Me = networkPlayer;
        if (player.Destination != player.Position)
            networkPlayer.Navigation.MoveTo(player.Position, player.Destination, false);

        networkPlayer.SetAppearance(player.Appearance);

        players.Add(networkPlayer);

        nametagManager.AddNameTag(networkPlayer);

    }

    public void OnPlayerRemoved(Player player)
    {
        Debug.Log("OnPlayerRemoved");

        var targetPlayer = players.FirstOrDefault(x => x.Id == player.Id);
        if (!targetPlayer)
        {
            Debug.LogError("Trying to remove a player that does not exist. ID: " + player.Id);
            return;
        }

        nametagManager.RemoveNameTag(targetPlayer);

        if (players.Remove(targetPlayer))
        {
            Destroy(targetPlayer.gameObject);
        }
    }

    internal void OnPlayerAnimationStateChanged(Player entity, string animationState, bool enabled, bool trigger, int action)
    {
        Debug.Log("OnPlayerAnimationStateChanged");
        var targetPlayer = players.FirstOrDefault(x => x.Id == entity.Id);
        if (!targetPlayer)
        {
            Debug.LogError("Trying to play an animation on a player that does not exist. ID: " + entity.Id);
            return;
        }

        targetPlayer.SetAnimationState(animationState, enabled, trigger, action);
    }

    internal void OnPlayerStatsUpdated(Player entity, decimal[] experience, int[] effectiveLevel)
    {
        Debug.Log("OnPlayerStatsUpdated");
        var targetPlayer = players.FirstOrDefault(x => x.Id == entity.Id);
        if (!targetPlayer)
        {
            Debug.LogError("Trying to update skill stat on a player that does not exist. ID: " + entity.Id);
            return;
        }

        targetPlayer.SetStats(experience, effectiveLevel);
    }

    internal void OnPlayerStatUpdated(Player entity, int skill, int level, int effectiveLevel, decimal experience)
    {
        Debug.Log("OnPlayerStatUpdated");
        var targetPlayer = players.FirstOrDefault(x => x.Id == entity.Id);
        if (!targetPlayer)
        {
            Debug.LogError("Trying to update skill stat on a player that does not exist. ID: " + entity.Id);
            return;
        }

        targetPlayer.UpdateStat(skill, level, effectiveLevel, experience);
    }

    internal void OnPlayerLevelUp(Player entity, int skill, int gainedLevels)
    {
        Debug.Log("OnPlayerLevelUp");
        var targetPlayer = players.FirstOrDefault(x => x.Id == entity.Id);
        if (!targetPlayer)
        {
            Debug.LogError("Trying to play an animation on a player that does not exist. ID: " + entity.Id);
            return;
        }

        targetPlayer.PlayLevelUpAnimation(skill, gainedLevels);
    }

    internal void OnPlayerEquipmentStateChanged(Player entity, int itemId, bool equipped)
    {
        Debug.Log("OnPlayerEquipmentStateChanged");
        var targetPlayer = players.FirstOrDefault(x => x.Id == entity.Id);
        if (!targetPlayer)
        {
            Debug.LogError("Trying to equip an item on a player that does not exist. ID: " + entity.Id);
            return;
        }

        targetPlayer.SetEquipmentState(itemId, equipped);
    }

    internal void OnPlayerInventoryUpdated(Player entity, long coins, int[] itemId, long[] amount)
    {
        Debug.Log("OnPlayerInventoryUpdated");
        var targetPlayer = players.FirstOrDefault(x => x.Id == entity.Id);
        if (!targetPlayer)
        {
            Debug.LogError("Trying to set inventory items for a player that does not exist. ID: " + entity.Id);
            return;
        }

        if (itemId != null && amount != null)
        {
            targetPlayer.SetInventoryItems(itemId, amount);
        }

        targetPlayer.SetCoins(coins);
    }

    internal void OnPlayerItemAdded(Player entity, int itemId, int amount)
    {
        Debug.Log("OnPlayerItemAdded");
        var targetPlayer = players.FirstOrDefault(x => x.Id == entity.Id);
        if (!targetPlayer)
        {
            Debug.LogError("Trying to add an item to a player that does not exist. ID: " + entity.Id);
            return;
        }

        targetPlayer.AddInventoryItem(itemId, amount);
    }

    internal void OnPlayerItemRemoved(Player entity, int itemId, int amount)
    {
        Debug.Log("OnPlayerItemRemoved");
        var targetPlayer = players.FirstOrDefault(x => x.Id == entity.Id);
        if (!targetPlayer)
        {
            Debug.LogError("Trying to remove an item from player that does not exist. ID: " + entity.Id);
            return;
        }

        targetPlayer.RemoveInventoryItem(itemId, amount);
    }

    internal void ResetState()
    {
        foreach (var player in players)
        {
            Destroy(player.gameObject);
        }

        players.Clear();
    }

    internal void OnPlayerHealthChanged(Player player, int health, int maxHealth, int delta)
    {
        Debug.Log("OnPlayerHealthChanged");
        var targetPlayer = players.FirstOrDefault(x => x.Id == player.Id);
        if (!targetPlayer)
        {
            return;
        }
        healthCounterManager.ShowCounter(targetPlayer.transform, delta);
        targetPlayer.SetHealth(health, maxHealth);
    }

    internal void OnPlayerNpcAction(
        Player player,
        int npcId,
        int actionType,
        int parameterId,
        byte status)
    {
        Debug.Log("OnPlayerNpcAction Response From Server");

        var targetPlayer = players.FirstOrDefault(x => x.Id == player.Id);
        if (!targetPlayer)
        {
            return;
        }
    }

    internal void OnPlayerObjectAction(
        Player player,
        int objectId,
        int actionType,
        int parameterId,
        byte status)
    {
        Debug.Log("OnPlayerAction Response From Server");

        var targetPlayer = players.FirstOrDefault(x => x.Id == player.Id);
        if (!targetPlayer)
        {
            return;
        }
    }
}
