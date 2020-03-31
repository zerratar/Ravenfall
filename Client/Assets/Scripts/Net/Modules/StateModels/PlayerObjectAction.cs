using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerObjectAction : EntityAction<Player>
    {
        public PlayerObjectAction(
            Player player,
            int objectId,
            int actionType,
            int parameterId,
            byte status)
            : base(player)
        {
            ObjectId = objectId;
            ActionType = actionType;
            ParameterId = parameterId;
            Status = status;
        }

        public int ObjectId { get; }
        public int ActionType { get; }
        public int ParameterId { get; }
        public byte Status { get; }
    }
}
