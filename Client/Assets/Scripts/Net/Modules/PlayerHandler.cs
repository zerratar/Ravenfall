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

        public void Move(int playerId, Vector3 position, Vector3 destination)
        {
            UnityEngine.Debug.Log("PlayerHandler::MovePlayer");

            lock (SyncRoot)
            {
                var targetPlayer = Entities.FirstOrDefault(x => x.Id == playerId);
                if (targetPlayer != null)
                {
                    targetPlayer.Destination = destination;
                    targetPlayer.Position = position;
                    Changes.Enqueue(new EntityMoved<Player>(targetPlayer));
                }
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

        public void Action(int playerId, int objectId, int actionType, int parameterId, byte status)
        {
            UnityEngine.Debug.Log("PlayerHandler::Action");
            var targetPlayer = GetPlayer(playerId);
            if (targetPlayer != null)
            {
                Changes.Enqueue(new PlayerObjectAction(targetPlayer, objectId, actionType, parameterId, status));
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
