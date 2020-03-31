using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private ServerItem[] items;

    public ServerItem GetItemById(int id)
    {
        if (items == null) return null;
        return items.FirstOrDefault(x => x.Id == id);
    }
}
