using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class NpcHandler : EntityHandler<Npc>
    {
        public NpcHandler()
            : base((a, b) => a.Id == b.Id)
        {
        }

        public override string Name => "Npc Handler";

        internal void Respawn(int npcServerId)
        {
            var npc = GetNPC(npcServerId);
            if (npc != null)
            {
                Changes.Enqueue(new NpcRespawned(npc));
            }
        }

        internal void Death(int npcServerId)
        {
            var npc = GetNPC(npcServerId);
            if (npc != null)
            {
                Changes.Enqueue(new NpcDied(npc));
            }
        }

        internal void UpdateHealth(int npcServerId, int health, int maxHealth, int delta)
        {
            var npc = GetNPC(npcServerId);
            if (npc != null)
            {
                Changes.Enqueue(new NpcHealthUpdated(npc, health, maxHealth, delta));
            }
        }

        public void SetAnimationState(int npcServerId, string animationState, bool enabled, bool trigger, int actionNumber)
        {
            var targetPlayer = GetNPC(npcServerId);

            if (targetPlayer != null)
            {
                Changes.Enqueue(new NpcAnimationStateUpdated(targetPlayer, animationState, enabled, trigger, actionNumber));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Npc GetNPC(int npcServerId)
        {
            lock (SyncRoot)
            {
                return Entities.FirstOrDefault(x => x.Id == npcServerId);
            }
        }

    }
}
