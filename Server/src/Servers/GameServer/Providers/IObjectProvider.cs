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
        SceneObjectAction GetAction(SceneObject serverObject, int actionId);
        void ReleaseObjectLocks(Player player);
        bool AcquireObjectLock(SceneObject obj, Player player);
        bool HasAcquiredObjectLock(SceneObject obj, Player player);
    }
}