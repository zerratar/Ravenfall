using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private ServerItem[] items;
    private void Awake()
    {
        items = Resources.LoadAll<ServerItem>("Data/Items");
    }
    public ServerItem GetItemById(int id)
    {
        if (items == null) return null;
        return items.FirstOrDefault(x => x.Id == id);
    }
}
