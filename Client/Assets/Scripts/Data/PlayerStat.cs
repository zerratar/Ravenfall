using System;

[Serializable]
public class PlayerStat
{
    private static volatile int index;

    public int Id;
    public string Name;
    public decimal Experience;
    public int Level;
    public int EffectiveLevel;

    public int AddExperience(decimal amount)
    {
        var delta = GameMath.ExperienceToLevel(Experience) - this.Level;
        this.Experience += amount;
        this.Level += delta;
        this.EffectiveLevel += delta;
        return delta;
    }

    public static PlayerStat Create(string name, int level = 1, decimal experience = 0)
    {
        var levelExp = GameMath.LevelToExperience(level);
        return new PlayerStat
        {
            Id = index++,
            Name = name,
            Level = level,
            EffectiveLevel = level,
            Experience = experience > 0 ? experience : levelExp
        };
    }

    internal void Set(int effective, int level)
    {
        this.EffectiveLevel = effective;
        this.Level = level;
    }
}