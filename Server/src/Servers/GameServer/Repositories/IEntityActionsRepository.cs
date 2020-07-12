
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Repositories
{
    public interface IEntityActionsRepository
    {
        IReadOnlyList<EntityActions> GetActions(EntityType type);
    }
}
