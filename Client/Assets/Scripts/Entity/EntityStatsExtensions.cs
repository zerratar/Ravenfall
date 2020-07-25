using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class EntityStatsExtensions
{
    private static Dictionary<string, PropertyInfo> attributesProps = new Dictionary<string, PropertyInfo>();
    private static Dictionary<string, PropertyInfo> professionsProps = new Dictionary<string, PropertyInfo>();

    public static int AddExperience(this EntityStats entity, string skill, decimal amount)
    {
        var experience = GetExperience(entity, skill);
        var level = GetLevel(entity, skill);
        var delta = GameMath.ExperienceToLevel(experience) - level;

        experience += amount;
        level += delta;

        SetExperience(entity, skill, experience);
        SetLevel(entity, skill, level);

        return delta;
    }

    public static decimal GetExperience(this EntityStats entity, string name)
    {
        return GetValue<decimal>(entity, name + "exp");
    }

    public static int GetLevel(this EntityStats entity, string name)
    {
        return GetValue<int>(entity, name);
    }

    public static void SetLevel(this EntityStats entity, string skill, int level)
    {
        SetValue(entity, skill, level);
    }

    public static void SetExperience(this EntityStats entity, string skill, decimal experience)
    {
        SetValue(entity, skill + "exp", experience);
    }

    private static void SetValue<T>(this EntityStats entity, string skill, T value)
    {
        var attributes = entity.Attributes;
        var attr = GetAttributeProperty(skill);
        if (attr != null)
        {
            attr.SetValue(attributes, value);
            return;
        }

        var professions = entity.Professions;
        var prof = GetProfessionProperty(skill);
        if (prof != null)
        {
            prof.SetValue(professions, value);
        }
    }

    private static T GetValue<T>(this EntityStats entity, string name)
    {
        var attributes = entity.Attributes;
        var attr = GetAttributeProperty(name);
        if (attr != null)
        {
            return (T)attr.GetValue(attributes);
        }

        var professions = entity.Professions;
        var prof = GetProfessionProperty(name);
        if (prof != null)
        {
            return (T)prof.GetValue(professions);
        }

        return default;
    }

    private static PropertyInfo GetProfessionProperty(string name)
    {
        if (professionsProps.Count == 0)
        {
            professionsProps = typeof(Professions).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(x => x.Name.ToLower(), y => y);
        }
        if (professionsProps.TryGetValue(name.ToLower(), out var prop))
        {
            return prop;
        }
        return null;
    }

    private static PropertyInfo GetAttributeProperty(string name)
    {
        if (attributesProps.Count == 0)
        {
            attributesProps = typeof(Attributes).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(x => x.Name.ToLower(), y => y);
        }
        if (attributesProps.TryGetValue(name.ToLower(), out var attr))
        {
            return attr;
        }
        return null;
    }

}
