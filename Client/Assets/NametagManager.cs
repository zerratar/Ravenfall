using System.Collections.Concurrent;
using UnityEngine;

public class NametagManager : MonoBehaviour
{
    [SerializeField] private GameObject nametagPrefab;
    [SerializeField] private GameObject nametagContainer;

    private readonly ConcurrentDictionary<string, Nametag> nametags
        = new ConcurrentDictionary<string, Nametag>();


    public void AddNameTag(NetworkPlayer player)
    {
        var nametagObject = Instantiate(nametagPrefab, nametagContainer.transform);
        var nametag = nametagObject.GetComponent<Nametag>() ?? nametagObject.AddComponent<Nametag>();
        nametag.SetPlayer(player);

        nametags[player.name] = nametag;
    }

    internal void RemoveNameTag(NetworkPlayer player)
    {
        if (nametags.TryRemove(player.name, out var nameTag))
        {
            Destroy(nameTag.gameObject);
        }
    }
}
