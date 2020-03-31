using Shinobytes.Ravenfall.DataModels;
using System;
using System.Collections.Generic;

namespace RavenNest.BusinessLogic.Data
{
    public interface IGameData
    {
        GameClient Client { get; }

        #region Find
        User FindUser(Func<User, bool> predicate);
        Character FindCharacter(Func<Character, bool> predicate);
        GameSession FindSession(Func<GameSession, bool> predicate);
        User FindUser(string userIdOrUsername);
        Village GetVillageBySession(GameSession session);
        Village GetOrCreateVillageBySession(GameSession session);
        IReadOnlyList<VillageHouse> GetOrCreateVillageHouses(Village village);

        /// <summary>
        /// Find player items by predicate
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IReadOnlyList<InventoryItem> FindPlayerItems(Guid characterId, Func<InventoryItem, bool> predicate);
        InventoryItem FindPlayerItem(Guid characterId, Func<InventoryItem, bool> predicate);
        GameSession GetSessionByUserId(string userId);

        #endregion

        #region Get
        User GetUser(Guid userId);
        IReadOnlyList<User> GetUsers();
        IReadOnlyList<Character> GetCharacters(Func<Character, bool> predicate);
        IReadOnlyList<Character> GetSessionCharacters(GameSession currentSession);
        User GetUser(string twitchUserId);
        int GetMarketItemCount();
        int GetNextGameEventRevision(Guid sessionId);
        GameSession GetSession(Guid sessionId, bool updateSession = true);
        GameSession GetUserSession(Guid userId, bool updateSession = true);
        IReadOnlyList<GameEvent> GetSessionEvents(GameSession gameSession);
        IReadOnlyList<Item> GetItems();
        Item GetItem(Guid id);
        IReadOnlyList<InventoryItem> GetInventoryItems(Guid characterId, Guid itemId);

        IReadOnlyList<InventoryItem> GetInventoryItems(Guid characterId);

        InventoryItem GetInventoryItem(Guid characterId, Guid itemId);
        InventoryItem GetEquippedItem(Guid characterId, Guid itemId);
        Character GetCharacter(Guid characterId);
        Character GetCharacterByUserId(Guid userId);
        Character GetCharacterByUserId(string twitchUserId);

        IReadOnlyList<MarketItem> GetMarketItems(Guid itemId);
        IReadOnlyList<MarketItem> GetMarketItems(int skip, int take);
        IReadOnlyList<GameEvent> GetSessionEvents(Guid sessionId);
        /// <summary>
        /// Gets all player items, this includes both equipped and items in the inventory.
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        IReadOnlyList<InventoryItem> GetAllPlayerItems(Guid characterId);

        /// <summary>
        /// Gets all equipped player items
        /// </summary>
        /// <param name="characterId"></param>
        /// <returns></returns>
        IReadOnlyList<InventoryItem> GetEquippedItems(Guid characterId);

        #endregion

        #region Create
        GameSession CreateSession(Guid userId);
        GameEvent CreateSessionEvent<T>(GameEventType type, GameSession session, T data);
        #endregion

        #region Add
        void Add(Item entity);
        void Add(CharacterState entity);
        void Add(SyntyAppearance entity);
        void Add(Statistics entity);
        void Add(Skills entity);
        void Add(Appearance entity);
        void Add(Resources entity);
        void Add(Character entity);
        void Add(User entity);
        void Add(InventoryItem entity);
        void Add(GameSession entity);
        void Add(MarketItem entity);
        void Add(GameEvent entity);
        void Add(Village village);

        /// <summary>
        ///     Force save the current state to the database.
        /// </summary>
        void Flush();

        #endregion

        //#region Update
        //void Update(CharacterState state);
        //void Update(Resources resources);
        //void Update(InventoryItem itemToEquip);
        //void Update(Skills skills);
        //void Update(SyntyAppearance syntyAppearance);
        //void Update(Appearance appearance);
        //void Update(MarketItem marketItem);
        //void Update(GameSession session);
        //void Update(Character character);
        //void UpdateRange(IEnumerable<InventoryItem> weapons);
        //#endregion

        #region Remove
        void Remove(User user);
        void Remove(Skills skills);
        void Remove(Statistics statistics);
        void Remove(Character character);
        void Remove(Resources resources);
        void Remove(MarketItem marketItem);
        void Remove(InventoryItem invItem);
        void RemoveRange(IReadOnlyList<InventoryItem> items);
        Resources GetResources(Guid resourcesId);
        Resources GetResourcesByCharacterId(Guid sellerCharacterId);
        Statistics GetStatistics(Guid statisticsId);
        Clan GetClan(Guid clanId);

        SyntyAppearance GetAppearance(Guid? syntyAppearanceId);
        Skills GetSkills(Guid skillsId);
        CharacterState GetState(Guid? stateId);
        #endregion

        IReadOnlyList<GameSession> GetActiveSessions();
        IReadOnlyList<ItemCraftingRequirement> GetCraftingRequirements(Guid itemId);
        CharacterSessionState GetCharacterSessionState(Guid sessionId, Guid characterId);
        SessionState GetSessionState(Guid sessionId);

        InventoryItem GetEquippedItem(Guid id, ItemCategory category);

        object SyncLock { get; }
    }
}
