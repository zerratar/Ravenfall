
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Repositories
{
    public class JsonBasedNpcRepository : INpcRepository
    {
        private readonly JsonBasedRepository<Npc> npcRepo;

        public JsonBasedNpcRepository()
        {
            npcRepo = new JsonBasedRepository<Npc>();
        }

        public IReadOnlyList<Npc> AllNpcs()
        {
            return npcRepo.All();
        }
    }
}
