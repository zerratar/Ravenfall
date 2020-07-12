using RavenfallServer.Providers;
using Shinobytes.Ravenfall.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Runtime.CompilerServices;

public class NpcAttackAction : EntityAction
{
    private const float MeleeRange = 3f;

    private const int PlayerMeleeAttackIntervalMs = 1500;
    private const int NpcMeleeAttackIntervalMs = 1500;
    private const int RetryTimeMs = 250;
    private const int TotalRetryTimeMs = 1500;

    private const string AttackAnimationName = "Attacking";

    private const string HealthSkill = "Health";

    private readonly IWorldProcessor worldProcessor;
    private readonly INpcStatsProvider npcStatsProvider;
    private readonly INpcStateProvider npcStateProvider;
    private readonly IPlayerStatsProvider playerStatsProvider;
    private readonly IPlayerStateProvider playerState;

    public NpcAttackAction(
        IWorldProcessor worldProcessor,
        INpcStatsProvider npcStatsProvider,
        INpcStateProvider npcStateProvider,
        IPlayerStatsProvider playerStatsProvider,
        IPlayerStateProvider playerStateProvider)
        : base(10, "Npc Attack")
    {
        this.worldProcessor = worldProcessor;
        this.npcStatsProvider = npcStatsProvider;
        this.npcStateProvider = npcStateProvider;
        this.playerStatsProvider = playerStatsProvider;
        this.playerState = playerStateProvider;
    }

    public override bool Invoke(
        Player player,
        Entity obj,
        int attackType)
    {
        if (!(obj is Npc npc))
        {
            return false;
        }

        playerState.SetAttackType(player, attackType);

        return Invoke(player, npc, TimeSpan.Zero, TimeSpan.Zero);
    }

    private bool Invoke(
        Player player,
        Npc npc,
        TimeSpan totalTime,
        TimeSpan deltaTime)
    {
        var attackType = playerState.GetAttackType(player);
        if (!npcStateProvider.IsEnemy(player, npc))
        {
            return false;
        }

        if (IsDead(npc))
        {
            return false;
        }

        if (!WithinDistance(player, npc, attackType))
        {
            if (totalTime.TotalMilliseconds < TotalRetryTimeMs)
                worldProcessor.SetEntityTimeout(RetryTimeMs, player, npc, Invoke);
            return false;
        }

        if (!ReadyForAttack(player, npc, attackType))
        {
            return false;
        }

        StartAnimation(player, npc, attackType);

        return false;
    }

    private void StartAnimation(Player player, Npc npc, int attackType)
    {
        playerState.EnterCombat(player, npc);
        playerState.UpdateAttackTime(player, npc);

        worldProcessor.PlayAnimation(player, AttackAnimationName, true, true, GetAttackAnimationNumber(attackType));
        worldProcessor.PlayAnimation(npc, AttackAnimationName, true, true);

        worldProcessor.SetEntityTimeout(PlayerMeleeAttackIntervalMs, player, npc, OnPlayerAfflictDamage);
        worldProcessor.SetEntityTimeout(NpcMeleeAttackIntervalMs, player, npc, OnNpcAfflictDamage);
    }

    private bool OnNpcAfflictDamage(
        Player player,
        Npc npc,
        TimeSpan totalTime,
        TimeSpan deltaTime)
    {

        return false;
    }


    private bool OnPlayerAfflictDamage(
        Player player,
        Npc npc,
        TimeSpan totalTime,
        TimeSpan deltaTime)
    {
        var attackType = playerState.GetAttackType(player);

        if (!playerState.InCombat(player, npc))
        {
            StopAnimation(player, npc, attackType);
            return false;
        }

        var damage = CalculateDamage(player.Id, npc.Id, playerStatsProvider, npcStatsProvider);
        var playerTrainingSkill = playerStatsProvider.GetStatByName(player.Id, "Attack"); // NEEDS TO BE UPDATED!!!
        var targetHealth = GetHealth(npc);

        targetHealth.EffectiveLevel -= damage;

        worldProcessor.NpcDamage(player, npc, damage, targetHealth.EffectiveLevel, targetHealth.Level);

        if (targetHealth.EffectiveLevel <= 0)
        {
            // he ded
            worldProcessor.NpcDeath(player, npc);
            worldProcessor.SetEntityTimeout(npc.RespawnTimeMs, player, npc, OnRespawn);

            var npcCombatLevel = npcStatsProvider.GetCombatLevel(npc.Id);
            var experience = GameMath.CombatExperience(npcCombatLevel);
            var levelsGaiend = playerTrainingSkill.AddExperience(experience);

            //var itemDrops = npcProvider.GetItemDrops(npc);
            //foreach (var itemDrop in itemDrops)
            //{
            //    if (random.NextDouble() > itemDrop.DropChance)
            //        continue;
            //    worldProcessor.AddPlayerItem(player, itemProvider.GetItemById(itemDrop.ItemId));
            //}

            worldProcessor.UpdatePlayerStat(player, playerTrainingSkill);

            if (levelsGaiend > 0)
            {
                worldProcessor.PlayerStatLevelUp(player, playerTrainingSkill, levelsGaiend);
            }

            StopAnimation(player, npc, attackType);
            return true;
        }

        StartAnimation(player, npc, attackType);
        return false;
    }

    private int CalculateDamage(
        int aEntityId,
        int bEntityId,
        IEntityStatsProvider aProvider,
        IEntityStatsProvider bProvider)
    {
        var aAttack = aProvider.GetStatByName(aEntityId, "Attack");
        var aStrength = aProvider.GetStatByName(aEntityId, "Defense");
        var aWeaponPower = 0;
        var aWeaponAim = 0;
        var aCombatStyle = 0;

        var bStrength = bProvider.GetStatByName(bEntityId, "Strength");
        var bDefense = bProvider.GetStatByName(bEntityId, "Defense");
        var bArmorPower = 0;
        var bCombatStyle = 0;

        return (int)GameMath.CalculateDamage(
            aAttack.EffectiveLevel, aStrength.EffectiveLevel, aCombatStyle, aWeaponPower, aWeaponAim,
            bStrength.EffectiveLevel, bDefense.EffectiveLevel, bCombatStyle, bArmorPower,
            aProvider is IPlayerStatsProvider,
            bProvider is INpcStatsProvider);
    }

    private bool OnRespawn(Player player, Npc npc, TimeSpan totalTime, TimeSpan deltaTime)
    {
        var healthStat = GetHealth(npc);
        healthStat.EffectiveLevel = healthStat.Level;
        worldProcessor.NpcRespawn(player, npc);
        return true;
    }
    private void StopAnimation(Player player, Npc npc, int attackType)
    {
        playerState.ExitCombat(player);
        playerState.ClearAttackTime(player, npc);

        worldProcessor.PlayAnimation(player,
            AttackAnimationName, false, false,
            GetAttackAnimationNumber(attackType));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool IsDead(Npc npc)
    {
        return GetHealth(npc).EffectiveLevel <= 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private EntityStat GetHealth(Player player)
    {
        return playerStatsProvider.GetStatByName(player.Id, HealthSkill);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private EntityStat GetHealth(Npc player)
    {
        return npcStatsProvider.GetStatByName(player.Id, HealthSkill);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int GetAttackAnimationNumber(int attackType)
    {
        return 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool ReadyForAttack(Player player, Npc npc, int attackType)
    {
        var lastAttack = playerState.GetAttackTime(player, npc);
        var timeDelta = DateTime.UtcNow - lastAttack;
        return timeDelta >= TimeSpan.FromMilliseconds(PlayerMeleeAttackIntervalMs);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool WithinDistance(Player player, Npc npc, int attackType)
    {
        var delta = player.Position - npc.Position;
        var distance = delta.SqrtMagnitude;
        return distance <= MeleeRange; // change depending how player attacks.
    }
}