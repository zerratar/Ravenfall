using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class NpcAnimationStateUpdate
    {
        public const short OpCode = 43;
        public int NpcServerId { get; set; }
        public string AnimationState { get; set; }
        public bool Enabled { get; set; }
        public bool Trigger { get; set; }
        public int ActionNumber { get; set; }

        internal static NpcAnimationStateUpdate Create(Npc npc, string anim, bool enabled, bool trigger, int number)
        {
            return new NpcAnimationStateUpdate
            {
                NpcServerId = npc.Id,
                AnimationState = anim,
                Enabled = enabled,
                Trigger = trigger,
                ActionNumber = number
            };
        }
    }
}
