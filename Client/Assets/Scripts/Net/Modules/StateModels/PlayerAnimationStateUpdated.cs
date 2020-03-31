using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerAnimationStateUpdated : EntityUpdated<Player>
    {
        public PlayerAnimationStateUpdated(
            Player entity,
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
