using GameServer.Repositories;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Managers
{
    public abstract class EntityManager
    {
        protected readonly ConcurrentDictionary<int, Lazy<EntityAction>[]> entityActions
            = new ConcurrentDictionary<int, Lazy<EntityAction>[]>();

        private ConcurrentDictionary<string, Type> loadedActionTypes;

        // Both objectPlayerLocks and playerObjectLocks are used for bidirectional lookups
        // objectPlayerLocks: Key: objectId, Value: player
        private readonly ConcurrentDictionary<int, Player> entityPlayerLocks = new ConcurrentDictionary<int, Player>();
        // playerObjectLocks: Key: playerId, Value: objectId
        private readonly ConcurrentDictionary<int, int> playerEntityLocks = new ConcurrentDictionary<int, int>();
        private readonly IoC ioc;
        private readonly IEntityActionsRepository actionRepo;

        public EntityManager(IoC ioc, IEntityActionsRepository actionRepo)
        {
            this.ioc = ioc;
            this.actionRepo = actionRepo;
        }

        public bool AcquireLock(Entity obj, Player player)
        {
            if (entityPlayerLocks.TryGetValue(obj.Id, out var ownedPlayer))
            {
                return ownedPlayer.Id == player.Id;
            }

            playerEntityLocks[player.Id] = obj.Id;
            entityPlayerLocks[obj.Id] = player;
            return true;
        }

        public bool HasAcquiredLock(Entity obj, Player player)
        {
            if (entityPlayerLocks.TryGetValue(obj.Id, out var ownedPlayer))
            {
                return ownedPlayer.Id == player.Id;
            }

            return false;
        }

        public void ReleaseLocks(Player player)
        {
            if (playerEntityLocks.TryGetValue(player.Id, out var entityId))
            {
                playerEntityLocks.TryRemove(player.Id, out _);
                entityPlayerLocks.TryRemove(entityId, out _);
            }
        }

        protected void AddActions(EntityType type)
        {
            var actions = actionRepo.GetActions(type);
            foreach (var action in actions)
            {
                Type[] actionTypes = ResolveActionTypes(action.ActionTypes);
                RegisterActions(action.EntityId, actionTypes);
            }
        }

        protected Type[] ResolveActionTypes(string[] actionTypes)
        {
            if (loadedActionTypes == null)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                loadedActionTypes = new ConcurrentDictionary<string, Type>(
                    assemblies
                    .SelectMany(x => x.GetTypes().Where(x => typeof(EntityAction).IsAssignableFrom(x)))
                    .ToDictionary(x => x.FullName, y => y));
            }

            return actionTypes
                .Select(x => loadedActionTypes.TryGetValue(x, out var type) ? type : null)
                .ToArray();
        }

        private void RegisterActions(int entityId, params Type[] actionTypes)
        {
            var actions = new List<Lazy<EntityAction>>();
            foreach (var type in actionTypes)
            {
                ioc.RegisterShared(type, type);
                actions.Add(new Lazy<EntityAction>(() => (EntityAction)ioc.Resolve(type)));
            }

            entityActions[entityId] = actions.ToArray();
        }

    }
}