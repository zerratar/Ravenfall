using GameServer.Managers;
using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace GameServer.Processors
{
    public interface IEntityProcessor<T> where T : Entity
    {
        void Update(T item, IGameSession session, TimeSpan deltaTime);
    }
}