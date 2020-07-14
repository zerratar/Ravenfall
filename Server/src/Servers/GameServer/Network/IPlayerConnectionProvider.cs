using GameServer.Managers;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Collections.Generic;

namespace GameServer.Network
{
    public interface IPlayerConnectionProvider : IRavenConnectionProvider
    {
        PlayerConnection GetPlayerConnection(Player player);
        IEnumerable<PlayerConnection> GetAllActivePlayerConnections(IGameSession session);
        IEnumerable<PlayerConnection> GetConnectedActivePlayerConnections(IGameSession session);
    }
}
