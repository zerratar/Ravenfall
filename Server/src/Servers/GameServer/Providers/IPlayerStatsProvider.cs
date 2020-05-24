using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace RavenfallServer.Providers
{
    public interface IPlayerStatsProvider
    {
        IReadOnlyList<PlayerStat> GetStats(int playerId);
        PlayerStat GetStatByIndex(int playerId, int id);
        PlayerStat GetStatByName(int playerId, string name);
        int GetCombatLevel(int playerId);
    }
}