using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public interface IObjectManager
    {
        IReadOnlyList<WorldObject> GetAll();
        WorldObject Replace(int serverObjectId, int newObjectId);
        WorldObject Get(int objectServerId);
        EntityAction GetAction(WorldObject serverObject, int actionId);
        ObjectItemDrop[] GetItemDrops(WorldObject obj);
        void ReleaseLocks(Player player);
        bool AcquireLock(Entity obj, Player player);
        bool HasAcquiredLock(Entity obj, Player player);
    }
}