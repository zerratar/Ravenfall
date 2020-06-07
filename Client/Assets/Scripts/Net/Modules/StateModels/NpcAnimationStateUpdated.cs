using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class NpcAnimationStateUpdated : EntityUpdated<Npc>
    {
        public NpcAnimationStateUpdated(
            Npc entity,
            string animationState,
            bool enabled,
            bool trigger,
            int action
            ) : base(entity)
        {
            AnimationState = animationState;
            Enabled = enabled;
            Trigger = trigger;
            Action = action;
        }

        public string AnimationState { get; }
        public bool Enabled { get; }
        public bool Trigger { get; }
        public int Action { get; }
    }
}
