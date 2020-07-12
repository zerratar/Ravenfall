using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthCounterManager : MonoBehaviour
{
    [SerializeField] private GameObject healthChangePrefab;
    [SerializeField] private Transform container;
    
    private readonly List<HealthCounter> healthCounters = new List<HealthCounter>();

    internal void ShowCounter(Transform transform, int delta)
    {
        var counter = healthCounters.FirstOrDefault(x => !x.gameObject.activeInHierarchy);
        if (!counter)
        {
            counter = Instantiate(healthChangePrefab, container).GetComponent<HealthCounter>();
            healthCounters.Add(counter);
        }

        counter.SetValue(transform, delta);
    }
}
