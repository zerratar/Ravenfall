using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Providers
{
    public interface INpcStateProvider
    {
        NpcAlignment GetAlignment(Player player, Npc npc);
        bool IsEnemy(Player player, Npc npc);
        void ExitCombat(Npc npc);
        void EnterCombat(Npc npc, Player player);
    }
}