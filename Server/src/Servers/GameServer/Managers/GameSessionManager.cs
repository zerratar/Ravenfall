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
        private const string OpenWorldGameSessionKey = "$__OPEN_WORLD__$";
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

        public IGameSession Get(WorldObject obj)
        {
            return gameSessions.Values.FirstOrDefault(session => session.Objects.GetAll().Any(x => x == obj));
        }

        public IGameSession Get(Player player)
        {
            return gameSessions.Values.FirstOrDefault(session => session.Players.GetAll().Any(x => x.Id == player.Id));
        }

        public IGameSession Get(string sessionKey)
        {

            if (string.IsNullOrEmpty(sessionKey))
            {
                sessionKey = OpenWorldGameSessionKey;
            }

            if (gameSessions.TryGetValue(sessionKey, out var session))
            {
                return session;
            }
            var isOpenWorldSession = sessionKey == OpenWorldGameSessionKey;
            return gameSessions[sessionKey] = CreateGameSession(isOpenWorldSession);
        }

        public IReadOnlyList<IGameSession> GetAll()
        {
            return gameSessions.Values.ToList();
        }

        public IReadOnlyList<IGameSession> GetUnmonitoredSessions()
        {
            return gameSessions.Values.Where(x => x.Bot == null).ToList();
        }

        private IGameSession CreateGameSession(bool isOpenWorldSession)
        {
            var npcs = new NpcManager(ioc, npcRepo, itemManager, entityActionsRepo);
            var objects = new ObjectManager(ioc, objRepo, entityActionsRepo);
            var gameSession = new GameSession(npcs, objects, isOpenWorldSession);
            return gameSession;
        }
    }
}