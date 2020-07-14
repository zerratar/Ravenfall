using RavenfallServer.Providers;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public interface INpcManager
    {
        INpcShopInventoryProvider Inventories { get; }
        INpcStatsProvider Stats { get; }
        INpcStateProvider States { get; }

        IReadOnlyList<Npc> GetAll();
        Npc Get(int npcServerId);
        EntityAction GetAction(Npc npc, int actionId);
    }
}