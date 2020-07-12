
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Repositories
{
    public class JsonBasedEntityActionsRepository : IEntityActionsRepository
    {
        private readonly JsonBasedRepository<EntityActions> actionRepo;

        public JsonBasedEntityActionsRepository()
        {
            actionRepo = new JsonBasedRepository<EntityActions>();
        }

        public IReadOnlyList<EntityActions> GetActions(EntityType type)
        {
            return actionRepo
                .All()
                .Where(x => x.EntityType == type)
                .ToList();
        }
    }
}
