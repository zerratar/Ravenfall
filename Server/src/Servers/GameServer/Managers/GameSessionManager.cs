using GameServer.Repositories;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace GameServer.Managers
{
    public class GameSessionManager : IGameSessionManager
    {
        private readonly ConcurrentDictionary<string, IGameSession> gameSessions = new ConcurrentDictionary<string, IGameSession>();
        private readonly IoC ioc;
        private readonly INpcRepository npcRepo;
        private readonly IItemManager itemManager;
        private readonly IEntityActionsRepository entityActionsRepo;
        private readonly IWorldObjectRepository objRepo;

        public GameSessionManager(
            IoC ioc, 
            IItemManager itemManager, 
            INpcRepository npcRepo, 
            IWorldObjectRepository objRepo, 
            IEntityActionsRepository entityActionsRepo)
        {
            this.ioc = ioc;
            this.itemManager = itemManager;
            this.npcRepo = npcRepo;
            this.objRepo = objRepo;
            this.entityActionsRepo = entityActionsRepo;
        }

        public IGameSession Get(Npc npc)
        {
            return gameSessions.Values.FirstOrDefault(session => session.Npcs.GetAll().Any(x => x == npc));
        }

        public IGameSession Get(Player player)
        {
            return gameSessions.Values.FirstOrDefault(session => session.Players.GetAll().Any(x => x.Id == player.Id));
        }

        public IGameSession Get(string sessionKey)
        {
            if (gameSessions.TryGetValue(sessionKey, out var session))
            {
                return session;
            }

            return gameSessions[sessionKey] = CreateGameSession();
        }

        private IGameSession CreateGameSession()
        {
            var npcs = new NpcManager(ioc, npcRepo, itemManager, entityActionsRepo);
            var objects = new ObjectManager(ioc, objRepo, entityActionsRepo);
            return new GameSession(npcs, objects);
        }

        public IReadOnlyList<IGameSession> GetAll()
        {
            return gameSessions.Values.ToList();
        }
    }
}