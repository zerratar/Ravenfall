
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

        public PlayerStat GetStatById(int playerId, int id)
        {
            if (playerStats.TryGetValue(playerId, out var stats))
            {
                return stats.FirstOrDefault(x => x.Id == id);
            }

            CreatePlayerStats(playerId);

            return GetStatById(playerId, id);
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
            playerStats[playerId] = new PlayerStat[]
            {
                PlayerStat.Create("Attack"),
                PlayerStat.Create("Defense"),
                PlayerStat.Create("Strength"),
                PlayerStat.Create("Health", 10, 1000),
                PlayerStat.Create("Ranged"),
                PlayerStat.Create("Magic"),
                PlayerStat.Create("Woodcutting"),
                PlayerStat.Create("Mining"),
                PlayerStat.Create("Fishing"),
                PlayerStat.Create("Cooking"),
            };
        }
    }
}