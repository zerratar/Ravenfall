using GameServer.Repositories;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameServer.Managers
{
    public class ObjectManager : EntityManager, IObjectManager
    {
        private readonly ConcurrentDictionary<int, ObjectItemDrop[]> objectItemDrops
            = new ConcurrentDictionary<int, ObjectItemDrop[]>();

        private readonly List<Shinobytes.Ravenfall.RavenNet.Models.WorldObject> entities = new List<Shinobytes.Ravenfall.RavenNet.Models.WorldObject>();
        private readonly object mutex = new object();
        private readonly IoC ioc;
        private readonly IWorldObjectRepository objectRepository;
        private readonly IEntityActionsRepository actionRepo;
        private int index = 0;

        public ObjectManager(
            IoC ioc, IWorldObjectRepository objRepo, IEntityActionsRepository actionRepo)
            : base(ioc, actionRepo)

        {
            this.ioc = ioc;
            objectRepository = objRepo;
            this.actionRepo = actionRepo;

            AddGameObjects();
            AddObjectDrops();
            AddActions(EntityType.Object);
        }

        public ObjectItemDrop[] GetItemDrops(Shinobytes.Ravenfall.RavenNet.Models.WorldObject obj)
        {
            if (objectItemDrops.TryGetValue(obj.ObjectId, out var drops))
            {
                return drops;
            }

            return new ObjectItemDrop[0];
        }

        public Shinobytes.Ravenfall.RavenNet.Models.WorldObject Get(int objectServerId)
        {
            lock (mutex)
            {
                return entities.FirstOrDefault(x => x.Id == objectServerId);
            }
        }

        public EntityAction GetAction(Shinobytes.Ravenfall.RavenNet.Models.WorldObject obj, int actionId)
        {
            if (entityActions.TryGetValue(obj.ObjectId, out var actions))
            {
                return actions.Select(x => x.Value).FirstOrDefault(x => x.Id == actionId);
            }
            return null;
        }

        public IReadOnlyList<Shinobytes.Ravenfall.RavenNet.Models.WorldObject> GetAll()
        {
            lock (mutex)
            {
                return entities;
            }
        }

        public Shinobytes.Ravenfall.RavenNet.Models.WorldObject Replace(int serverObjectId, int newObjectId)
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

        protected void AddGameObjects()
        {
            var objects = objectRepository.AllObjects();

            foreach (var obj in objects)
            {
                obj.Id = Interlocked.Increment(ref index);
                entities.Add(obj);
            }
        }

        private void AddObjectDrops()
        {
            var drops = objectRepository.GetItemDrops();
            foreach (var drop in drops)
            {
                RegisterObjectItemDrop(drop.ObjectId, drop.Drops);
            }
        }
    }
}