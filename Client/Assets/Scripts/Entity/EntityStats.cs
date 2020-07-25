using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Concurrent;
using UnityEngine;
public class EntityStats : MonoBehaviour
{
    [SerializeField] private GameObject levelUpEffectPrefab;

    public Attributes Attributes { get; private set; }
    public Professions Professions { get; private set; }

    public int Health { get; set; }

    public int MaxHealth { get; set; }

    internal void PlayLevelUpAnimation(string skill, int gainedLevels)
    {
        if (levelUpEffectPrefab)
        {
            var levelupEffect = Instantiate(levelUpEffectPrefab, this.transform);

            levelupEffect.AddComponent<AutoDestroyPS>();
        }
    }

    internal void UpdateStat(string skill, int level, decimal experience)
    {
        this.SetLevel(skill, level);
        this.SetExperience(skill, experience);
    }

    internal void SetHealth(int health, int maxHealth)
    {
        Health = health;
        MaxHealth = maxHealth;
    }

    internal void SetStats(Attributes attributes, Professions professions)
    {
        Debug.Log("Got stats from server.");
        Attributes = attributes;
        Professions = professions;
    }
}
