using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class NpcDied : EntityUpdated<Npc>
    {
        public NpcDied(Npc entity) : base(entity) { }
    }
}
