using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server.Packets;

namespace Shinobytes.Ravenfall.ZoneServer
{
    public interface IZone
    {
        int NpcCount { get; }
        int PlayerCount { get; }

        PlayerEnterUpdate OnPlayerEnter(Player player);
        PlayerExitUpdate OnPlayerExit(int playerId);
    }
}
