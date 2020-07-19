using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public interface IGameSessionManager
    {
        IReadOnlyList<IGameSession> GetAll();
        IGameSession Get(Npc npc);
        IGameSession Get(Player player);
        IGameSession Get(WorldObject obj);
        IGameSession Get(string sessionKey);

        /// <summary>
        ///     Gets all <see cref="IGameSession"/> that has no <see cref="Shinobytes.Ravenfall.RavenNet.Server.IStreamBot"/> monitoring.
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<IGameSession> GetUnmonitoredSessions();
    }
}