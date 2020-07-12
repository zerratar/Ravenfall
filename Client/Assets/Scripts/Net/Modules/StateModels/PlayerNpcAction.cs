using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerNpcAction : EntityAction<Player>
    {
        public PlayerNpcAction(
            Player player,
            int npcId,
            int actionType,
            int parameterId,
            byte status)
            : base(player)
        {
            NpcId = npcId;
            ActionType = actionType;
            ParameterId = parameterId;
            Status = status;
        }

        public int NpcId { get; }
        public int ActionType { get; }
        public int ParameterId { get; }
        public byte Status { get; }
    }

}
