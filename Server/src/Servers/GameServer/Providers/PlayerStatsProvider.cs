
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RavenfallServer.Providers
{
    public class PlayerStatsProvider : IPlayerStatsProvider
    {
        private readonly ConcurrentDictionary<int, PlayerStat[]> playerStats
            = new ConcurrentDictionary<int, PlayerStat[]>();

        public int GetCombatLevel(int playerId)
        {
            var attack = GetStatByName(playerId, "Attack");
            var defense = GetStatByName(playerId, "Defense");
            var strength = GetStatByName(playerId, "Strength");
            var health = GetStatByName(playerId, "Health");
            var ranged = GetStatByName(playerId, "Ranged");
            var magic = GetStatByName(playerId, "Magic");

            return (int)Math.Floor((attack.Level + defense.Level + strength.Level + health.Level) / 4f + (magic.Level + ranged.Level) / 8f);
        }

        public PlayerStat GetStatByIndex(int playerId, int index)
        {
            if (playerStats.TryGetValue(playerId, out var stats))
            {
                //return stats.FirstOrDefault(x => x.Id == index);
                return stats[index];
            }

            CreatePlayerStats(playerId);

            return GetStatByIndex(playerId, index);
        }

        public PlayerStat GetStatByName(int playerId, string name)
        {
            if (playerStats.TryGetValue(playerId, out var stats))
            {
                return stats.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }

            CreatePlayerStats(playerId);

            return GetStatByName(playerId, name);
        }

        public IReadOnlyList<PlayerStat> GetStats(int playerId)
        {
            if (playerStats.TryGetValue(playerId, out var stats))
            {
                return stats.ToList();
            }

            CreatePlayerStats(playerId);

            return GetStats(playerId);
        }

        private void CreatePlayerStats(int playerId)
        {
            var index = 0;
            playerStats[playerId] = new PlayerStat[]
            {
                PlayerStat.Create(index++, "Attack"),
                PlayerStat.Create(index++, "Defense"),
                PlayerStat.Create(index++, "Strength"),
                PlayerStat.Create(index++, "Health", 10, 1000),
                PlayerStat.Create(index++, "Ranged"),
                PlayerStat.Create(index++, "Magic"),
                PlayerStat.Create(index++, "Woodcutting"),
                PlayerStat.Create(index++, "Mining"),
                PlayerStat.Create(index++, "Fishing"),
                PlayerStat.Create(index++, "Cooking"),
            };
        }
    }
}