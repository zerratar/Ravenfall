using Shinobytes.Ravenfall.Data.Entities;
using System;
using System.Collections.Generic;

namespace Shinobytes.Ravenfall.DataModels
{
    public partial class Statistics : Entity<Statistics>
    {

        private Guid id; public Guid Id { get => id; set => Set(ref id, value); }

        private int raidsWon; public int RaidsWon { get => raidsWon; set => Set(ref raidsWon, value); }
        private int raidsLost; public int RaidsLost { get => raidsLost; set => Set(ref raidsLost, value); }
        private int raidsJoined; public int RaidsJoined { get => raidsJoined; set => Set(ref raidsJoined, value); }

        private int duelsWon; public int DuelsWon { get => duelsWon; set => Set(ref duelsWon, value); }
        private int duelsLost; public int DuelsLost { get => duelsLost; set => Set(ref duelsLost, value); }

        private int playersKilled; public int PlayersKilled { get => playersKilled; set => Set(ref playersKilled, value); }
        private int enemiesKilled; public int EnemiesKilled { get => enemiesKilled; set => Set(ref enemiesKilled, value); }

        private int arenaFightsJoined; public int ArenaFightsJoined { get => arenaFightsJoined; set => Set(ref arenaFightsJoined, value); }
        private int arenaFightsWon; public int ArenaFightsWon { get => arenaFightsWon; set => Set(ref arenaFightsWon, value); }

        private long totalDamageDone; public long TotalDamageDone { get => totalDamageDone; set => Set(ref totalDamageDone, value); }
        private long totalDamageTaken; public long TotalDamageTaken { get => totalDamageTaken; set => Set(ref totalDamageTaken, value); }
        private int deathCount; public int DeathCount { get => deathCount; set => Set(ref deathCount, value); }

        private decimal totalWoodCollected; public decimal TotalWoodCollected { get => totalWoodCollected; set => Set(ref totalWoodCollected, value); }
        private decimal totalOreCollected; public decimal TotalOreCollected { get => totalOreCollected; set => Set(ref totalOreCollected, value); }
        private decimal totalFishCollected; public decimal TotalFishCollected { get => totalFishCollected; set => Set(ref totalFishCollected, value); }
        private decimal totalWheatCollected; public decimal TotalWheatCollected { get => totalWheatCollected; set => Set(ref totalWheatCollected, value); }

        private int craftedWeapons; public int CraftedWeapons { get => craftedWeapons; set => Set(ref craftedWeapons, value); }
        private int craftedArmors; public int CraftedArmors { get => craftedArmors; set => Set(ref craftedArmors, value); }
        private int craftedPotions; public int CraftedPotions { get => craftedPotions; set => Set(ref craftedPotions, value); }
        private int craftedRings; public int CraftedRings { get => craftedRings; set => Set(ref craftedRings, value); }
        private int craftedAmulets; public int CraftedAmulets { get => craftedAmulets; set => Set(ref craftedAmulets, value); }

        private int cookedFood; public int CookedFood { get => cookedFood; set => Set(ref cookedFood, value); }

        private int consumedPotions; public int ConsumedPotions { get => consumedPotions; set => Set(ref consumedPotions, value); }
        private int consumedFood; public int ConsumedFood { get => consumedFood; set => Set(ref consumedFood, value); }

        private long totalTreesCutDown; public long TotalTreesCutDown { get => totalTreesCutDown; set => Set(ref totalTreesCutDown, value); }

        //public ICollection<Character> Character { get; set; }
    }
}