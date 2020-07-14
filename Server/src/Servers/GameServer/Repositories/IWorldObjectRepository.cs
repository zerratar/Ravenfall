
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Repositories
{
    public interface IWorldObjectRepository
    {
        IReadOnlyList<WorldObject> AllObjects();
        IReadOnlyList<WorldObjectItemDrops> GetItemDrops();
    }
}
