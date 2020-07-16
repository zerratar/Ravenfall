using System;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;

namespace GameServer.Processors
{
    public delegate bool EntityTickHandler<TObject>(Player player, TObject obj, TimeSpan totalTime, TimeSpan deltaTime);

    public interface IWorldProcessor
    {
        void RemovePlayer(Player player);
        void AddPlayer(string sessionKey, PlayerConnection connection);
        void PlayerObjectInteraction(Player player, WorldObject serverObject, EntityAction action, int parameterId);
        void PlayAnimation(Player player, string animation, bool enabled = true, bool trigger = false, int number = 0);
        void PlayAnimation(Npc npc, string animation, bool enabled = true, bool trigger = false, int number = 0);
        void SetItemEquipState(Player player, Item item, bool v);
        void SendChatMessage(Player player, int channelID, string message);
        void UpdatePlayerStat(Player player, EntityStat skill);
        void PlayerStatLevelUp(Player player, EntityStat skill, int levelsGaiend);
        void AddPlayerItem(Player player, Item item, int amount = 1);
        void RemovePlayerItem(Player player, Item item, int amount = 1);
        void OpenTradeWindow(Player player, Npc npc, string shopName, ShopInventory shopInventory);
        void PlayerNpcInteraction(Player player, Npc npc, EntityAction action, int parameterId);
        void PlayerBuyItem(Player player, Npc npc, int itemId, int amount);
        void PlayerSellItem(Player player, Npc npc, int itemId, int amount);
        void NpcDamage(Player player, Npc npc, int damage, int health, int maxHealth);
        void NpcDeath(Player player, Npc npc);
        void NpcRespawn(Player player, Npc npc);
        void UpdateObject(WorldObject obj);
        void SetEntityInterval<TObject>(
            Player player,
            TObject tree,
            EntityTickHandler<TObject> handleObjectTick)
            where TObject : Entity;

        ITimeoutHandle SetEntityTimeout<TObject>(
            int milliseconds,
            Player player,
            TObject obj,
            EntityTickHandler<TObject> handleObjectTick)
            where TObject : Entity;
        void ClearEntityTimeout(ITimeoutHandle handle);
    }
}