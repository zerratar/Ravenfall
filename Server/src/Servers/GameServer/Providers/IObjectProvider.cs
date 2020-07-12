using RavenfallServer.Objects;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace RavenfallServer.Providers
{
    public interface IObjectProvider
    {
        IReadOnlyList<SceneObject> GetAll();
        SceneObject Replace(int serverObjectId, int newObjectId);
        SceneObject Get(int objectServerId);
        EntityAction GetAction(SceneObject serverObject, int actionId);
        ObjectItemDrop[] GetItemDrops(SceneObject obj);
        void ReleaseLocks(Player player);
        bool AcquireLock(Entity obj, Player player);
        bool HasAcquiredLock(Entity obj, Player player);
    }
}