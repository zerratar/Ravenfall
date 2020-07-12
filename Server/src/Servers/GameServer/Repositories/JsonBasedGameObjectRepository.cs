
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Repositories
{
    public class JsonBasedGameObjectRepository : IGameObjectRepository
    {
        private readonly JsonBasedRepository<SceneObject> objectRepo;
        private readonly JsonBasedRepository<SceneObjectItemDrops> itemDropRepo;

        public JsonBasedGameObjectRepository()
        {
            objectRepo = new JsonBasedRepository<SceneObject>();
            itemDropRepo = new JsonBasedRepository<SceneObjectItemDrops>();
        }

        public IReadOnlyList<SceneObject> AllObjects()
        {
            return objectRepo.All();
        }

        public IReadOnlyList<SceneObjectItemDrops> GetItemDrops()
        {
            return itemDropRepo.All();
        }
    }
}
