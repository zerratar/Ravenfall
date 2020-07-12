
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Repositories
{
    public interface IGameObjectRepository
    {
        IReadOnlyList<SceneObject> AllObjects();
        IReadOnlyList<SceneObjectItemDrops> GetItemDrops();
    }
}
