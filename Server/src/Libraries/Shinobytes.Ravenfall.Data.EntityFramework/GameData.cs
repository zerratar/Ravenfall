using Shinobytes.Ravenfall.Core;
using Shinobytes.Ravenfall.Data;
using Shinobytes.Ravenfall.Data.Entities;
using Shinobytes.Ravenfall.Data.EntityFramework;
using Shinobytes.Ravenfall.DataModels;
using Shinobytes.Ravenfall.RavenNet.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
namespace RavenNest.BusinessLogic.Data
{
    public class GameData : IGameData
    {
        private const int SaveInterval = 10000;
        private const int SaveMaxBatchSize = 100;

        private readonly IRavenfallDbContextProvider db;
        private readonly ILogger logger;
        private readonly IKernel kernel;
        private readonly IQueryBuilder queryBuilder;

        private readonly ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, CharacterSessionState>> characterSessionStates
            = new ConcurrentDictionary<Guid, ConcurrentDictionary<Guid, CharacterSessionState>>();

        private readonly ConcurrentDictionary<Guid, SessionState> sessionStates
            = new ConcurrentDictionary<Guid, SessionState>();

        private readonly EntitySet<Appearance, Guid> appearances;
        private readonly EntitySet<SyntyAppearance, Guid> syntyAppearances;
        private readonly EntitySet<Character, Guid> characters;
        private readonly EntitySet<CharacterState, Guid> characterStates;
        private readonly EntitySet<GameSession, Guid> gameSessions;
        private readonly EntitySet<GameEvent, Guid> gameEvents;
        private readonly EntitySet<InventoryItem, Guid> inventoryItems;
        private readonly EntitySet<MarketItem, Guid> marketItems;
        private readonly EntitySet<Item, Guid> items;
        private readonly EntitySet<NPC, Guid> npcs;
        private readonly EntitySet<NPCItemDrop, Guid> npcItemDrops;
        private readonly EntitySet<NPCSpawn, Guid> npcSpawns;
        private readonly EntitySet<ItemCraftingRequirement, Guid> itemCraftingRequirements;
        private readonly EntitySet<Resources, Guid> resources;
        private readonly EntitySet<Statistics, Guid> statistics;
        private readonly EntitySet<Skills, Guid> skills;
        private readonly EntitySet<User, Guid> users;
        private readonly EntitySet<GameClient, Guid> gameClients;
        private readonly EntitySet<Clan, Guid> clans;
        private readonly EntitySet<Village, Guid> villages;
        private readonly EntitySet<VillageHouse, Guid> villageHouses;

        private readonly IEntitySet[] entitySets;

        private ITimeoutHandle scheduleHandler;
        public object SyncLock { get; } = new object();

        public GameData(IRavenfallDbContextProvider db, ILogger logger, IKernel kernel, IQueryBuilder queryBuilder)
        {
            this.db = db;
            this.logger = logger;
            this.kernel = kernel;
            this.queryBuilder = queryBuilder;

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            using (var ctx = this.db.Get())
            {
                appearances = new EntitySet<Appearance, Guid>(ctx.Appearance.ToList(), i => i.Id);
                syntyAppearances = new EntitySet<SyntyAppearance, Guid>(ctx.SyntyAppearance.ToList(), i => i.Id);

                characters = new EntitySet<Character, Guid>(ctx.Character.ToList(), i => i.Id);
                characters.RegisterLookupGroup(nameof(User), x => x.UserId);
                characters.RegisterLookupGroup(nameof(GameSession), x => x.UserIdLock.GetValueOrDefault());

                characterStates = new EntitySet<CharacterState, Guid>(ctx.CharacterState.ToList(), i => i.Id);
                gameSessions = new EntitySet<GameSession, Guid>(ctx.GameSession.ToList(), i => i.Id);
                gameSessions.RegisterLookupGroup(nameof(User), x => x.UserId);

                // we can still store the game events, but no need to load them on startup as the DB will quickly be filled.
                // and take a long time to load
                gameEvents = new EntitySet<GameEvent, Guid>(new List<GameEvent>() /*ctx.GameEvent.ToList()*/, i => i.Id);
                gameEvents.RegisterLookupGroup(nameof(GameSession), x => x.GameSessionId);

                inventoryItems = new EntitySet<InventoryItem, Guid>(ctx.InventoryItem.ToList(), i => i.Id);
                inventoryItems.RegisterLookupGroup(nameof(Character), x => x.CharacterId);

                marketItems = new EntitySet<MarketItem, Guid>(ctx.MarketItem.ToList(), i => i.Id);
                marketItems.RegisterLookupGroup(nameof(Item), x => x.ItemId);

                items = new EntitySet<Item, Guid>(ctx.Item.ToList(), i => i.Id);

                npcs = new EntitySet<NPC, Guid>(ctx.NPC.ToList(), i => i.Id);
                npcSpawns = new EntitySet<NPCSpawn, Guid>(ctx.NPCSpawn.ToList(), i => i.Id);
                npcSpawns.RegisterLookupGroup(nameof(NPC), x => x.NpcId);

                npcItemDrops = new EntitySet<NPCItemDrop, Guid>(ctx.NPCItemDrop.ToList(), i => i.Id);
                npcItemDrops.RegisterLookupGroup(nameof(NPC), x => x.NpcId);

                itemCraftingRequirements = new EntitySet<ItemCraftingRequirement, Guid>(ctx.ItemCraftingRequirement.ToList(), i => i.Id);
                itemCraftingRequirements.RegisterLookupGroup(nameof(Item), x => x.ItemId);
                //itemCraftingRequirements.RegisterLookupGroup(nameof(ItemCraftingRequirement.ResourceItemId), x => x.ItemId);

                clans = new EntitySet<Clan, Guid>(ctx.Clan.ToList(), i => i.Id);
                villages = new EntitySet<Village, Guid>(ctx.Village.ToList(), i => i.Id);
                villages.RegisterLookupGroup(nameof(User), x => x.UserId);

                villageHouses = new EntitySet<VillageHouse, Guid>(ctx.VillageHouse.ToList(), i => i.Id);
                villageHouses.RegisterLookupGroup(nameof(Village), x => x.VillageId);

                resources = new EntitySet<Resources, Guid>(ctx.Resources.ToList(), i => i.Id);
                statistics = new EntitySet<Statistics, Guid>(ctx.Statistics.ToList(), i => i.Id);
                skills = new EntitySet<Skills, Guid>(ctx.Skills.ToList(), i => i.Id);
                users = new EntitySet<User, Guid>(ctx.User.ToList(), i => i.Id);

                gameClients = new EntitySet<GameClient, Guid>(ctx.GameClient.ToList(), i => i.Id);

                Client = gameClients.Entities.First();

                entitySets = new IEntitySet[]
                {
                    appearances, syntyAppearances, characters, characterStates,
                    gameSessions, gameEvents, inventoryItems, marketItems, items,
                    resources, statistics, skills, users, gameClients, villages, villageHouses, clans,
                    npcs, npcSpawns, npcItemDrops
                };
            }
            stopWatch.Stop();
            logger.Debug($"All database entries loaded in {stopWatch.Elapsed.TotalSeconds} seconds.");
        }

        public GameClient Client { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Add(VillageHouse house) => Update(() => villageHouses.Add(house));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Village entity) => Update(() => villages.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Item entity) => Update(() => items.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(CharacterState entity) => Update(() => characterStates.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(SyntyAppearance entity) => Update(() => syntyAppearances.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Statistics entity) => Update(() => statistics.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Skills entity) => Update(() => skills.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Appearance entity) => Update(() => appearances.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Resources entity) => Update(() => resources.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(Character entity) => Update(() => characters.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(User entity) => Update(() => users.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(InventoryItem entity) => Update(() => inventoryItems.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(GameSession entity) => Update(() => gameSessions.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(MarketItem entity) => Update(() => marketItems.Add(entity));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(GameEvent entity) => Update(() => gameEvents.Add(entity));

        public GameSession CreateSession(Guid userId)
        {
            return new GameSession
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Revision = 0,
                Started = DateTime.UtcNow,
                Status = (int)SessionStatus.Active
            };
        }

        public GameEvent CreateSessionEvent<T>(GameEventType type, GameSession session, T data)
        {
            session.Updated = DateTime.UtcNow;
            return new GameEvent
            {
                Id = Guid.NewGuid(),
                GameSessionId = session.Id,
                Type = (int)type,
                Revision = GetNextGameEventRevision(session.Id),
                Data = JSON.Stringify(data)
            };
        }

        // This is not code, it is a shrimp. Cant you see?
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Character FindCharacter(Func<Character, bool> predicate) =>
            characters.Entities.FirstOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public InventoryItem FindPlayerItem(Guid id, Func<InventoryItem, bool> predicate) =>
            characters.TryGet(id, out var player)
                ? inventoryItems[nameof(Character), player.Id].FirstOrDefault(predicate)
                : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<InventoryItem> FindPlayerItems(Guid id, Func<InventoryItem, bool> predicate) =>
            characters.TryGet(id, out var player)
                ? inventoryItems[nameof(Character), player.Id].Where(predicate).ToList()
                : null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameSession FindSession(Func<GameSession, bool> predicate) =>
            gameSessions.Entities.FirstOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameSession GetSessionByUserId(string userId)
        {
            var user = users.Entities.FirstOrDefault(x => x.UserId == userId);
            if (user == null) return null;

            var character = characters.Entities.FirstOrDefault(x => x.UserId == user.Id);
            if (character == null) return null;

            return gameSessions.Entities
                .FirstOrDefault(x =>
                    x.UserId == character.UserIdLock &&
                    (SessionStatus)x.Status == SessionStatus.Active);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public User FindUser(Func<User, bool> predicate) =>
            users.Entities.FirstOrDefault(predicate);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public User FindUser(string userIdOrUsername) =>
            users.Entities.FirstOrDefault(x =>
                    x.UserId == userIdOrUsername ||
                    x.UserName.Equals(userIdOrUsername, StringComparison.OrdinalIgnoreCase));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<InventoryItem> GetAllPlayerItems(Guid characterId) =>
            inventoryItems[nameof(Character), characterId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<InventoryItem> GetInventoryItems(Guid characterId) =>
            inventoryItems[nameof(Character), characterId].Where(x => !x.Equipped).ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Character GetCharacter(Guid characterId) =>
            characters[characterId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Character GetCharacterByUserId(Guid userId) =>
            characters[nameof(User), userId].FirstOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Character GetCharacterByUserId(string twitchUserId)
        {
            var user = GetUser(twitchUserId);
            return user == null ? null : characters[nameof(User), user.Id].FirstOrDefault();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<ItemCraftingRequirement> GetCraftingRequirements(Guid itemId) => itemCraftingRequirements[nameof(Item), itemId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<Character> GetCharacters(Func<Character, bool> predicate) =>
            characters.Entities.Where(predicate).ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<User> GetUsers() =>
            users.Entities.ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public InventoryItem GetEquippedItem(Guid characterId, Guid itemId) =>
            inventoryItems[nameof(Character), characterId]
                .FirstOrDefault(x => x.Equipped && x.ItemId == itemId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public InventoryItem GetInventoryItem(Guid characterId, Guid itemId) =>
            inventoryItems[nameof(Character), characterId]
               .FirstOrDefault(x => !x.Equipped && x.ItemId == itemId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<InventoryItem> GetEquippedItems(Guid characterId) =>
            inventoryItems[nameof(Character), characterId]
                    .Where(x => x.Equipped)
                    .ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<InventoryItem> GetInventoryItems(Guid characterId, Guid itemId) =>
            inventoryItems[nameof(Character), characterId]
                    .Where(x => !x.Equipped && x.ItemId == itemId)
                    .ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Item GetItem(Guid id) => items[id];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<Item> GetItems() => items.Entities.ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetMarketItemCount() => marketItems.Entities.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<MarketItem> GetMarketItems(Guid itemId) =>
            marketItems[nameof(Item), itemId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<MarketItem> GetMarketItems(int skip, int take) =>
            marketItems.Entities.Skip(skip).Take(take).ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetNextGameEventRevision(Guid sessionId)
        {
            var events = GetSessionEvents(sessionId);
            if (events.Count == 0) return 1;
            return events.Max(x => x.Revision) + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameSession GetSession(Guid sessionId, bool updateSession = true)
        {
            var session = gameSessions[sessionId];
            if (updateSession && session != null) session.Updated = DateTime.UtcNow;
            return session;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<Character> GetSessionCharacters(GameSession currentSession) =>
            characters[nameof(GameSession), currentSession.UserId]
                    .Where(x => x.LastUsed > currentSession.Started)
                    .ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<GameEvent> GetSessionEvents(GameSession gameSession) =>
            GetSessionEvents(gameSession.Id);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<GameEvent> GetSessionEvents(Guid sessionId) =>
            gameEvents[nameof(GameSession), sessionId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public User GetUser(Guid userId) => users[userId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public User GetUser(string twitchUserId) => users.Entities
                .FirstOrDefault(x =>
                    x.UserName.Equals(twitchUserId, StringComparison.OrdinalIgnoreCase) ||
                    x.UserId.Equals(twitchUserId, StringComparison.OrdinalIgnoreCase));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GameSession GetUserSession(Guid userId, bool updateSession = true)
        {
            var session = gameSessions[nameof(User), userId]
                    .OrderByDescending(x => x.Started)
                    .FirstOrDefault(x => x.Stopped == null);
            if (updateSession && session != null) session.Updated = DateTime.UtcNow;
            return session;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(User user) => users.Remove(user);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(Skills skill) => skills.Remove(skill);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(Statistics stat) => statistics.Remove(stat);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(Character character) => characters.Remove(character);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(Resources res) => resources.Remove(res);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(MarketItem marketItem) => marketItems.Remove(marketItem);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove(InventoryItem invItem) => inventoryItems.Remove(invItem);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveRange(IReadOnlyList<InventoryItem> items) => items.ForEach(Remove);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Resources GetResources(Guid resourcesId) => resources[resourcesId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Resources GetResourcesByCharacterId(Guid sellerCharacterId) =>
            GetResources(GetCharacter(sellerCharacterId).ResourcesId);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Statistics GetStatistics(Guid statisticsId) => statistics[statisticsId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SyntyAppearance GetAppearance(Guid? syntyAppearanceId) =>
            syntyAppearanceId == null ? null : syntyAppearances[syntyAppearanceId.Value];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Skills GetSkills(Guid skillsId) => skills[skillsId];


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Clan GetClan(Guid clanId) => clans[clanId];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CharacterState GetState(Guid? stateId) => stateId == null ? null : characterStates[stateId.Value];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<GameSession> GetActiveSessions() => gameSessions.Entities
                    .OrderByDescending(x => x.Started)
                    .Where(x => x.Stopped == null).ToList();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public InventoryItem GetEquippedItem(Guid characterId, ItemCategory category)
        {
            foreach (var invItem in inventoryItems[nameof(Character), characterId].Where(x => x.Equipped))
            {
                var item = GetItem(invItem.ItemId);
                if (item.Category == (int)category) return invItem;
            }
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Village GetVillageBySession(GameSession session)
            => villages[nameof(User), session.UserId].FirstOrDefault();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Village GetOrCreateVillageBySession(GameSession session)
        {
            var village = GetVillageBySession(session);
            if (village == null)
            {
                var villageResources = new Resources()
                {
                    Id = Guid.NewGuid()
                };

                Add(villageResources);


                var user = GetUser(session.UserId);
                var villageExp = user.IsAdmin.GetValueOrDefault()
                    ? GameMath.LevelToExperience(30)
                    : 0;
                var villageLevel = GameMath.ExperienceToLevel(villageExp);

                village = new Village()
                {
                    Id = Guid.NewGuid(),
                    ResourcesId = villageResources.Id,
                    Level = villageLevel,
                    Experience = (long)villageExp,
                    Name = "Village",
                    UserId = session.UserId
                };

                Add(village);
            }

            return village;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<VillageHouse> GetOrCreateVillageHouses(Village village)
        {
            var houses = villageHouses[nameof(Village), village.Id];

            if (village.Level <= 1)
            {
                return new VillageHouse[0];
            }

            var houseCount = village.Level / 10;

            if ((houses == null || houses.Count == 0) && houseCount > 0)
            {
                houses = Enumerable.Range(0, houseCount).Select(x => new VillageHouse
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    Slot = x,
                    Type = 0,
                    VillageId = village.Id
                }).ToList();

                foreach (var house in houses)
                {
                    Add(house);
                }
            }

            if (houses.Count < houseCount)
            {
                var housesTemp = houses.ToList();

                for (var i = houses.Count; i < houseCount; ++i)
                {
                    var house = new VillageHouse
                    {
                        Id = Guid.NewGuid(),
                        Created = DateTime.UtcNow,
                        Slot = i,
                        Type = 0,
                        VillageId = village.Id
                    };

                    Add(house);

                    housesTemp.Add(house);
                }

                houses = housesTemp;
            }
            return houses;
        }

        public CharacterSessionState GetCharacterSessionState(Guid sessionId, Guid characterId)
        {
            ConcurrentDictionary<Guid, CharacterSessionState> states;

            if (!characterSessionStates.TryGetValue(sessionId, out states))
            {
                states = new ConcurrentDictionary<Guid, CharacterSessionState>();
            }

            CharacterSessionState state;
            if (!states.TryGetValue(characterId, out state))
            {
                state = new CharacterSessionState();
                states[characterId] = state;
                characterSessionStates[sessionId] = states;
            }

            return state;
        }

        public SessionState GetSessionState(Guid sessionId)
        {
            SessionState state;
            if (!sessionStates.TryGetValue(sessionId, out state))
            {
                state = new SessionState();
                sessionStates[sessionId] = state;
            }

            return state;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ScheduleNextSave()
        {
            if (scheduleHandler != null) return;
            scheduleHandler = kernel.SetTimeout(SaveChanges, SaveInterval);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Update(Action update)
        {
            if (update == null) return;
            update.Invoke();
            ScheduleNextSave();
        }

        public void Flush()
        {
            SaveChanges();
        }

        private void SaveChanges()
        {
            kernel.ClearTimeout(scheduleHandler);
            scheduleHandler = null;
            try
            {
                lock (SyncLock)
                {
                    logger.Debug("Saving all pending changes to the database.");

                    var queue = BuildSaveQueue();
                    using (var con = db.GetConnection())
                    {
                        con.Open();
                        while (queue.TryPeek(out var saveData))
                        {
                            var query = queryBuilder.Build(saveData);
                            if (query == null) return;

                            var command = con.CreateCommand();
                            command.CommandText = query.Command;

                            var result = command.ExecuteNonQuery();
                            if (result == 0)
                            {
                                logger.Error("Unable to save data! Abort Query failed");
                                return;
                            }

                            ClearChangeSetState(saveData);

                            queue.Dequeue();
                        }
                        con.Close();
                    }

                    //ClearChangeSetState();
                }
            }
            //catch (SqlException exc)
            //{
            //    foreach (SqlErrorCollection error in exc.Errors)
            //    {
            //        var saveError = ParseSqlError(error.ToString());

            //        HandleSqlError(saveError);
            //    }

            //    logger.WriteError("ERROR SAVING DATA!! " + exc);
            //}
            catch (Exception exc)
            {
                logger.Error("ERROR SAVING DATA!! " + exc);
                // log this
            }
            finally
            {
                ScheduleNextSave();
            }
        }

        private void HandleSqlError(DataSaveError saveError)
        {
        }

        private DataSaveError ParseSqlError(string error)
        {
            if (error.Contains("duplicate key"))
            {
                return ParseDuplicateKeyError(error);
            }

            if (error.Contains("insert the value NULL into"))
            {
                return ParseNullInsertError(error);
            }

            return null;
        }

        private DataSaveError ParseNullInsertError(string error)
        {
            return null;
            // TODO
        }

        private DataSaveError ParseDuplicateKeyError(string error)
        {
            var id = error.Split('(').Last().Split(')').First();
            var type = error.Split(new string[] { "'dbo." }, StringSplitOptions.None).Last().Split("'").First();
            return null;
            // TODO
        }

        private void ClearChangeSetState(EntityStoreItems items = null)
        {
            foreach (var set in entitySets)
            {
                if (items == null)
                    set.ClearChanges();
                else
                    set.Clear(items.Entities);
            }
        }

        private Queue<EntityStoreItems> BuildSaveQueue()
        {
            var queue = new Queue<EntityStoreItems>();
            var addedItems = JoinChangeSets(entitySets.Select(x => x.Added).ToArray());
            foreach (var batch in CreateBatches(EntityState.Added, addedItems, SaveMaxBatchSize))
            {
                queue.Enqueue(batch);
            }

            var updateItems = JoinChangeSets(entitySets.Select(x => x.Updated).ToArray());
            foreach (var batch in CreateBatches(EntityState.Modified, updateItems, SaveMaxBatchSize))
            {
                queue.Enqueue(batch);
            }

            var deletedItems = JoinChangeSets(entitySets.Select(x => x.Removed).ToArray());
            foreach (var batch in CreateBatches(EntityState.Deleted, deletedItems, SaveMaxBatchSize))
            {
                queue.Enqueue(batch);
            }

            return queue;
        }

        private ICollection<EntityStoreItems> CreateBatches(EntityState state, ICollection<EntityChangeSet> items, int batchSize)
        {
            if (items == null || items.Count == 0) return new List<EntityStoreItems>();
            var batches = (int)Math.Floor(items.Count / (float)batchSize) + 1;
            var batchList = new List<EntityStoreItems>(batches);
            for (var i = 0; i < batches; ++i)
            {
                batchList.Add(new EntityStoreItems(state, items.Skip(i * batchSize).Take(batchSize).Select(x => x.Entity).ToList()));
            }
            return batchList;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ICollection<EntityChangeSet> JoinChangeSets(params ICollection<EntityChangeSet>[] changesets) =>
            changesets.SelectMany(x => x).OrderBy(x => x.LastModified).ToList();
    }

    public class DataSaveError
    {
    }
}
