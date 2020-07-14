using GameServer.Managers;
using System;

namespace GameServer.Processors
{
    public class GameSessionProcessor : IGameSessionProcessor
    {
        private readonly IPlayerProcessor playerProcessor;
        private readonly INpcProcessor npcProcessor;
        private readonly IObjectProcessor objProcessor;

        public GameSessionProcessor(
            IPlayerProcessor playerProcessor,
            INpcProcessor npcProcessor,
            IObjectProcessor objProcessor)
        {
            this.playerProcessor = playerProcessor;
            this.npcProcessor = npcProcessor;
            this.objProcessor = objProcessor;
        }

        public void Update(IGameSession session, TimeSpan deltaTime)
        {
            var players = session.Players.GetAll();
            foreach (var player in players)
            {
                playerProcessor.Update(player, session, deltaTime);
            }

            var npcs = session.Npcs.GetAll();
            foreach (var npc in npcs)
            {
                npcProcessor.Update(npc, session, deltaTime);
            }

            var objs = session.Objects.GetAll();
            foreach (var obj in objs)
            {
                objProcessor.Update(obj, session, deltaTime);
            }
        }
    }
}