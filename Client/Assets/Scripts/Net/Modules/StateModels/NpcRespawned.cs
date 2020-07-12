using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class NpcRespawned : EntityUpdated<Npc>
    {
        public NpcRespawned(Npc entity) : base(entity) { }
    }
}
