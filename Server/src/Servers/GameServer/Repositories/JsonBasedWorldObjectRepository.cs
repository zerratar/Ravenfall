
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Repositories
{
    public class JsonBasedWorldObjectRepository : IWorldObjectRepository
    {
        private readonly JsonBasedRepository<WorldObject> objectRepo;
        private readonly JsonBasedRepository<WorldObjectItemDrops> itemDropRepo;

        public JsonBasedWorldObjectRepository()
        {
            objectRepo = new JsonBasedRepository<WorldObject>();
            itemDropRepo = new JsonBasedRepository<WorldObjectItemDrops>();
        }

        public IReadOnlyList<WorldObject> AllObjects()
        {
            return objectRepo.All();
        }

        public IReadOnlyList<WorldObjectItemDrops> GetItemDrops()
        {
            return itemDropRepo.All();
        }
    }
}
