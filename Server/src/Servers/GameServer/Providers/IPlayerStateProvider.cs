using Shinobytes.Ravenfall.RavenNet.Models;
using System;

namespace RavenfallServer.Providers
{
    public interface IEntityStateProvider
    {
        T GetState<T>(Entity entity, string key);
        void SetState<T>(Entity entity, string key, T model);
        void RemoveState(Entity entity, string key);
    }

    public interface IPlayerStateProvider : IEntityStateProvider
    {
        void UpdateAttackTime(Player player, Npc npc);

        void ClearAttackTime(Player player, Npc npc);

        DateTime GetAttackTime(Player player, Npc opponent);

        void ExitCombat(Player player);

        void EnterCombat(Player player, Npc opponent);
        void EnterCombat(Player player, Player opponent);

        bool InCombat(Player player, Npc opponent);
        bool InCombat(Player player, Player opponent);

        void SetAttackType(Player player, int attackType);
        int GetAttackType(Player player);
    }
}