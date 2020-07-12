using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerHandler : EntityHandler<Player>
    {
        public PlayerHandler()
            : base((a, b) => a.Id == b.Id)
        {
        }

        public override string Name => "Player Handler";

        public void Move(int playerId, Vector3 position, Vector3 destination, bool running)
        {
            lock (SyncRoot)
            {
                var targetPlayer = Entities.FirstOrDefault(x => x.Id == playerId);
                if (targetPlayer != null)
                {
                    targetPlayer.Destination = destination;
                    targetPlayer.Position = position;
                    targetPlayer.Running = running;
                    Changes.Enqueue(new EntityMoved<Player>(targetPlayer));
                }
            }
        }

        internal void UpdateHealth(int targetPlayerId, int health, int maxHealth, int delta)
        {
            lock (SyncRoot)
            {
                var targetPlayer = Entities.FirstOrDefault(x => x.Id == targetPlayerId);
                if (targetPlayer != null)
                {
                    Changes.Enqueue(new PlayerHealthUpdated(targetPlayer, health, maxHealth, delta));
                }
            }
        }

        internal void SetPlayerInventory(int playerId, long coins, int[] itemId, long[] amount)
        {
            var targetPlayer = GetPlayer(playerId);
            if (targetPlayer != null)
            {
                Changes.Enqueue(new PlayerInventoryUpdated(targetPlayer, coins, itemId, amount));
                return;
            }
        }

        internal void PlayerStatsUpdate(int playerId, decimal[] experience, int[] effectiveLevel)
        {
            var targetPlayer = GetPlayer(playerId);
            if (targetPlayer != null)
            {
                Changes.Enqueue(new PlayerStatsUpdated(targetPlayer, experience, effectiveLevel));
                return;
            }
        }

        internal void PlayerItemAdd(int playerId, int itemId, int amount)
        {
            var targetPlayer = GetPlayer(playerId);
            if (targetPlayer != null)
            {
                Changes.Enqueue(new PlayerItemAdded(targetPlayer, itemId, amount));
                return;
            }
        }
        internal void PlayerItemRemove(int playerId, int itemId, int amount)
        {
            var targetPlayer = GetPlayer(playerId);
            if (targetPlayer != null)
            {
                Changes.Enqueue(new PlayerItemRemoved(targetPlayer, itemId, amount));
                return;
            }
        }

        internal void PlayerStatUpdate(int playerId, int skill, int level, int effectiveLevel, decimal experience)
        {
            var targetPlayer = GetPlayer(playerId);
            if (targetPlayer != null)
            {
                Changes.Enqueue(new PlayerStatUpdated(targetPlayer, skill, level, effectiveLevel, experience));
            }
        }

        internal void PlayerLevelUp(int playerId, int skill, int gainedLevels)
        {
            var targetPlayer = GetPlayer(playerId);
            if (targetPlayer != null)
            {
                Changes.Enqueue(new PlayerLeveledUp(targetPlayer, skill, gainedLevels));
            }
        }

        internal void SetEquimentState(int playerId, int itemId, bool equipped)
        {
            var targetPlayer = GetPlayer(playerId);
            if (targetPlayer != null)
            {
                Changes.Enqueue(new PlayerEquipmentStateUpdated(targetPlayer, itemId, equipped));
            }
        }

        public void SetAnimationState(int playerId, string animationState, bool enabled, bool trigger, int actionNumber)
        {
            var targetPlayer = GetPlayer(playerId);

            if (targetPlayer != null)
            {
                Changes.Enqueue(new PlayerAnimationStateUpdated(targetPlayer, animationState, enabled, trigger, actionNumber));
            }
        }

        public void OpenTradeWindow(int playerId, int npcServerId, string shopName, int[] itemId, int[] itemPrice, int[] itemStock)
        {
            UnityEngine.Debug.Log("PlayerHandler::ObjectAction");
            var targetPlayer = GetPlayer(playerId);
            if (targetPlayer != null)
            {
                Changes.Enqueue(new OpenNpcTradeWindow(targetPlayer, npcServerId, shopName, itemId, itemPrice, itemStock));
            }
        }

        public void ObjectAction(int playerId, int objectId, int actionType, int parameterId, byte status)
        {
            UnityEngine.Debug.Log("PlayerHandler::ObjectAction");
            var targetPlayer = GetPlayer(playerId);
            if (targetPlayer != null)
            {
                Changes.Enqueue(new PlayerObjectAction(targetPlayer, objectId, actionType, parameterId, status));
            }
        }

        public void NpcAction(int playerId, int npcId, int actionType, int parameterId, byte status)
        {
            UnityEngine.Debug.Log("PlayerHandler::NpcAction");
            var targetPlayer = GetPlayer(playerId);
            if (targetPlayer != null)
            {
                Changes.Enqueue(new PlayerNpcAction(targetPlayer, npcId, actionType, parameterId, status));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Player GetPlayer(int playerId)
        {
            lock (SyncRoot)
            {
                return Entities.FirstOrDefault(x => x.Id == playerId);
            }
        }
    }
}
