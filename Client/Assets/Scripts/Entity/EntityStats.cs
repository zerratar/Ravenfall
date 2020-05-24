using System;
using System.Linq;
using UnityEngine;
public class EntityStats : MonoBehaviour
{
    [SerializeField] private GameObject levelUpEffectPrefab;

    public readonly PlayerStat[] Stats = new PlayerStat[]
    {
        PlayerStat.Create("Attack"),
        PlayerStat.Create("Defense"),
        PlayerStat.Create("Strength"),
        PlayerStat.Create("Health", 10, 1000),
        PlayerStat.Create("Ranged"),
        PlayerStat.Create("Magic"),
        PlayerStat.Create("Woodcutting"),
        PlayerStat.Create("Mining"),
        PlayerStat.Create("Fishing"),
        PlayerStat.Create("Cooking"),
    };

    internal void PlayLevelUpAnimation(int skill, int gainedLevels)
    {
        if (levelUpEffectPrefab)
        {
            var levelupEffect = Instantiate(levelUpEffectPrefab, this.transform);
            
            levelupEffect.AddComponent<AutoDestroyPS>();
        }

        var playerSkill = Stats[skill];
        if (playerSkill == null) return;

        Debug.Log("Congratulations, you've gained " + gainedLevels + " " + playerSkill.Name + " level(s)!");
    }

    internal void UpdateStat(int skill, int level, int effectiveLevel, decimal experience)
    {
        var playerSkill = Stats[skill];
        if (playerSkill == null) return;

        playerSkill.Level = level;
        playerSkill.EffectiveLevel = effectiveLevel;
        playerSkill.Experience = experience;
    }

    internal void SetStats(decimal[] experience, int[] effectiveLevel)
    {
        Debug.Log("Got stats from server: " + experience.Length + ", " + effectiveLevel.Length);
        for (var i = 0; i < experience.Length; ++i)
        {
            Stats[i].Experience = experience[i];
            Stats[i].EffectiveLevel = effectiveLevel[i];
            Stats[i].Level = GameMath.ExperienceToLevel(experience[i]);
        }
    }
}
