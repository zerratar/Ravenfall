using GameServer.Managers;
using System;

namespace GameServer.Processors
{
    public interface IGameSessionProcessor
    {
        void Update(IGameSession session, TimeSpan deltaTime);
    }
}