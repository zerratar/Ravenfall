namespace Shinobytes.Ravenfall.DataModels
{
    public enum GameEventType
    {
        Raid = 0,
        WarRaid = 1,
        PlayerAdd = 2,
        PlayerRemove = 3,
        PlayerAppearance = 4,
        PlayerTask = 5,
        PlayerNameUpdate = 16,
        PlayerExpUpdate = 17,
        PlayerJoinRaid = 18,
        PlayerJoinDungeon = 19,
        PlayerJoinArena = 20,
        PlayerMove = 21,
        PlayerAttack = 22,
        PlayerAction = 23,
        PlayerDuelAccept = 24,
        PlayerDuelDecline = 25,
        PlayerDuelRequest = 26,
        PlayerTravel = 27,
        ItemBuy = 6,
        ItemSell = 7,
        ItemEquip = 8,
        ItemUnEquip = 9,
        ItemAdd = 10,
        ResourceUpdate = 11,
        ServerMessage = 12,
        PermissionChange = 13,
        VillageLevelUp = 14,
        VillageInfo = 15
    }
}
