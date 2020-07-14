using GameServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RavenfallServer.Providers
{
    public class PlayerStateProvider : EntityStateProvider, IPlayerStateProvider
    {
        private const string AttackTypeKey = "AttackType";
        private const string AttackTimeStatePrefix = "AttackTime";
        private const string InCombatState = "InCombat";
        private const string TagNPC = "_Npc_";
        private const string TagPlayer = "_Player_";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetAttackType(Player player, int attackType)
        {
            SetState(player, AttackTypeKey, attackType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetAttackType(Player player)
        {
            return GetState<int>(player, AttackTypeKey);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public DateTime GetAttackTime(Player player, Npc npc)
        {
            return GetState<DateTime>(player, AttackTimeStatePrefix + TagNPC + npc.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateAttackTime(Player player, Npc npc)
        {
            SetState(player, AttackTimeStatePrefix + TagNPC + npc.Id, DateTime.UtcNow);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ClearAttackTime(Player player, Npc npc)
        {
            SetState(player, AttackTimeStatePrefix + TagNPC + npc.Id, DateTime.MinValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExitCombat(Player player)
        {
            foreach (var key in State.Keys.Where(x => x.IndexOf(InCombatState + TagNPC[0], 0, StringComparison.OrdinalIgnoreCase) >= 0))
            {
                State.TryRemove(key, out _);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnterCombat(Player player, Npc opponent)
        {
            SetState(player, InCombatState + TagNPC + opponent.Id, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool InCombat(Player player, Npc opponent)
        {
            return GetState<bool>(player, InCombatState + TagNPC + opponent.Id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnterCombat(Player player, Player opponent)
        {
            SetState(player, InCombatState + TagPlayer + opponent.Id, true);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool InCombat(Player player, Player opponent)
        {
            return GetState<bool>(player, InCombatState + TagPlayer + opponent.Id);
        }
    }
}