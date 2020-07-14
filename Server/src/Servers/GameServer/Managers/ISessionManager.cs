using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public interface IGameSessionManager
    {
        IReadOnlyList<IGameSession> GetAll();
        IGameSession Get(Npc npc);
        IGameSession Get(Player player);
        IGameSession Get(string sessionKey);

    }
}