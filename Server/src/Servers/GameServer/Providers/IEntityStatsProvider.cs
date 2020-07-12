using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace RavenfallServer.Providers
{
    public interface IEntityStatsProvider
    {
        IReadOnlyList<EntityStat> GetStats(int entityId);
        EntityStat GetStatByIndex(int entityId, int statsId);
        EntityStat GetStatByName(int entityId, string name);        
        int GetCombatLevel(int entityId);

        //EntityStat GetHealth(int entityId);
        //EntityStat GetAttack(int entityId);
        //EntityStat GetStrength(int entityId);
        //EntityStat GetDefense(int entityId);
        //EntityStat GetMagic(int entityId);
        //EntityStat GetRanged(int entityId);
    }
}