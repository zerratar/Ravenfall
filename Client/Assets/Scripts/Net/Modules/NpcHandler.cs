using Shinobytes.Ravenfall.RavenNet.Models;
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

        public void SetAnimationState(int npcId, string animationState, bool enabled, bool trigger, int actionNumber)
        {
            var targetPlayer = GetNPC(npcId);

            if (targetPlayer != null)
            {
                Changes.Enqueue(new NpcAnimationStateUpdated(targetPlayer, animationState, enabled, trigger, actionNumber));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Npc GetNPC(int npcId)
        {
            lock (SyncRoot)
            {
                return Entities.FirstOrDefault(x => x.Id == npcId);
            }
        }
    }
}
