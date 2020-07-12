using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace RavenfallServer.Providers
{
    public interface INpcProvider
    {
        IReadOnlyList<Npc> GetAll();
        Npc Get(int npcServerId);
        EntityAction GetAction(Npc npc, int actionId);
    }
}