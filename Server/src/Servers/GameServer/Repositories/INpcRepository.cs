
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;

namespace GameServer.Repositories
{
    public interface INpcRepository
    {
        IReadOnlyList<Npc> AllNpcs();
    }
}
