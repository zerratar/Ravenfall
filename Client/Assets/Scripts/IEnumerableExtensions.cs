using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IEnumerableExtensions
{
    public static IReadOnlyList<decimal> Delta(this IList<decimal> newValue, IReadOnlyList<decimal> oldValue)
    {
        if (oldValue == null)
        {
            return new List<decimal>(newValue.Count);
        }
        if (newValue.Count != oldValue.Count)
        {
            return new List<decimal>(newValue.Count);
        }

        return newValue.Select((x, i) => x - oldValue[i]).ToList();
    }

    public static IEnumerable<T> Except<T>(this IEnumerable<T> items, T except)
    {
        return items.Where(x => !x.Equals(except));
    }

    public static int RandomIndex<T>(this IEnumerable<T> items)
    {
        return Mathf.FloorToInt(UnityEngine.Random.value * items.Count());
    }
    public static T Random<T>(this IEnumerable<T> items)
    {
        var selections = items.ToList();
        return selections[Mathf.FloorToInt(UnityEngine.Random.value * selections.Count)];
    }
}