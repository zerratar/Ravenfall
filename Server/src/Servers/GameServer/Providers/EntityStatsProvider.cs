
using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace RavenfallServer.Providers
{
    public abstract class EntityStatsProvider : IEntityStatsProvider
    {
        private readonly ConcurrentDictionary<int, EntityStat[]> entityStats
        = new ConcurrentDictionary<int, EntityStat[]>();

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

        public EntityStat GetStatByIndex(int playerId, int index)
        {
            if (entityStats.TryGetValue(playerId, out var stats))
            {
                //return stats.FirstOrDefault(x => x.Id == index);
                return stats[index];
            }

            CreateEntityStats(playerId);

            return GetStatByIndex(playerId, index);
        }

        public EntityStat GetStatByName(int playerId, string name)
        {
            if (entityStats.TryGetValue(playerId, out var stats))
            {
                return stats.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            }

            CreateEntityStats(playerId);

            return GetStatByName(playerId, name);
        }

        public IReadOnlyList<EntityStat> GetStats(int playerId)
        {
            if (entityStats.TryGetValue(playerId, out var stats))
            {
                return stats.ToList();
            }

            CreateEntityStats(playerId);

            return GetStats(playerId);
        }

        private void CreateEntityStats(int playerId)
        {
            var index = 0;
            entityStats[playerId] = new EntityStat[]
            {
                EntityStat.Create(index++, "Attack"),
                EntityStat.Create(index++, "Defense"),
                EntityStat.Create(index++, "Strength"),
                EntityStat.Create(index++, "Health", 10, 1000),
                EntityStat.Create(index++, "Ranged"),
                EntityStat.Create(index++, "Magic"),
                EntityStat.Create(index++, "Woodcutting"),
                EntityStat.Create(index++, "Mining"),
                EntityStat.Create(index++, "Fishing"),
                EntityStat.Create(index++, "Cooking"),
            };
        }
    }
}