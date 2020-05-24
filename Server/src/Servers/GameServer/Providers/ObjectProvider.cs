using GameServer.Repositories;
using RavenfallServer.Objects;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RavenfallServer.Providers
{
    public partial class ObjectProvider : IObjectProvider
    {
        private readonly ConcurrentDictionary<int, Lazy<SceneObjectAction>[]> objectActions
            = new ConcurrentDictionary<int, Lazy<SceneObjectAction>[]>();

        private readonly ConcurrentDictionary<int, ObjectItemDrop[]> objectItemDrops
            = new ConcurrentDictionary<int, ObjectItemDrop[]>();

        // Both objectPlayerLocks and playerObjectLocks are used for bidirectional lookups
        // objectPlayerLocks: Key: objectId, Value: player
        private readonly ConcurrentDictionary<int, Player> objectPlayerLocks = new ConcurrentDictionary<int, Player>();
        // playerObjectLocks: Key: playerId, Value: objectId
        private readonly ConcurrentDictionary<int, int> playerObjectLocks = new ConcurrentDictionary<int, int>();

        private readonly List<SceneObject> entities = new List<SceneObject>();
        private readonly object mutex = new object();
        private readonly IoC ioc;
        private readonly IGameObjectRepository objectRepository;
        private int index = 0;

        public ObjectProvider(IoC ioc)
        {
            this.ioc = ioc;
            this.objectRepository = ioc.Resolve<IGameObjectRepository>();

            AddGameObjects();
            AddObjectDrops();
            AddObjectActions();

            //for (var i = 1; i <= 10; ++i)
            //{
            //    //RegisterObjectItemDrop(i, new ObjectItemDrop { DropChance = 1f, ItemId = 2 });
            //    RegisterObjectActions(i, typeof(TreeChopAction), typeof(ExamineAction));
            //}

            ////RegisterObjectItemDrop(50, new ObjectItemDrop { DropChance = 1f, ItemId = 3 });
            //RegisterObjectActions(50, typeof(RockPickAction), typeof(ExamineAction));

            ////RegisterObjectItemDrop(75, new ObjectItemDrop { DropChance = 1f, ItemId = 4 });
            //RegisterObjectActions(75, typeof(FishAction), typeof(ExamineAction));
        }

        public bool AcquireObjectLock(SceneObject obj, Player player)
        {
            if (objectPlayerLocks.TryGetValue(obj.Id, out var ownedPlayer))
            {
                return ownedPlayer.Id == player.Id;
            }

            playerObjectLocks[player.Id] = obj.Id;
            objectPlayerLocks[obj.Id] = player;
            return true;
        }

        public bool HasAcquiredObjectLock(SceneObject obj, Player player)
        {
            if (objectPlayerLocks.TryGetValue(obj.Id, out var ownedPlayer))
            {
                return ownedPlayer.Id == player.Id;
            }

            return false;
        }

        public ObjectItemDrop[] GetItemDrops(SceneObject obj)
        {
            if (objectItemDrops.TryGetValue(obj.ObjectId, out var drops))
            {
                return drops;
            }

            return new ObjectItemDrop[0];
        }

        public SceneObject Get(int objectServerId)
        {
            lock (mutex)
            {
                return entities.FirstOrDefault(x => x.Id == objectServerId);
            }
        }

        public SceneObjectAction GetAction(SceneObject obj, int actionId)
        {
            if (objectActions.TryGetValue(obj.ObjectId, out var actions))
            {
                return actions.Select(x => x.Value).FirstOrDefault(x => x.Id == actionId);
            }
            return null;
        }

        public IReadOnlyList<SceneObject> GetAll()
        {
            lock (mutex)
            {
                return entities;
            }
        }

        public void ReleaseObjectLocks(Player player)
        {
            if (playerObjectLocks.TryGetValue(player.Id, out var objectId))
            {
                playerObjectLocks.TryRemove(player.Id, out _);
                objectPlayerLocks.TryRemove(objectId, out _);
            }
        }

        public SceneObject Replace(int serverObjectId, int newObjectId)
        {
            lock (mutex)
            {
                var targetObject = entities.FirstOrDefault(x => x.Id == serverObjectId);
                if (targetObject == null) return null;
                targetObject.ObjectId = newObjectId;
                return targetObject;
            }
        }

        private void RegisterObjectItemDrop(int objectId, params ObjectItemDrop[] items)
        {
            objectItemDrops[objectId] = items;
        }

        private void RegisterObjectActions(int objectId, params Type[] actionTypes)
        {
            var actions = new List<Lazy<SceneObjectAction>>();
            foreach (var type in actionTypes)
            {
                ioc.RegisterShared(type, type);
                actions.Add(new Lazy<SceneObjectAction>(() => (SceneObjectAction)ioc.Resolve(type)));
            }

            objectActions[objectId] = actions.ToArray();
        }
    }
}