using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerStatsUpdated : EntityUpdated<Player>
    {
        public PlayerStatsUpdated(Player entity)
             : base(entity)
        {
        }
    }
}
