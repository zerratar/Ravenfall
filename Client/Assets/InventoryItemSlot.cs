using Shinobytes.Ravenfall.RavenNet.Models;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSlot : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI lblAmount;
    [SerializeField] private Image itemIcon;
    [SerializeField] private ItemManager itemManager;
    [SerializeField] private InventoryPanel inventoryPanel;

    private InventoryItem item;
    public InventoryItem Item => item;

    private void Start()
    {
        if (!itemManager) itemManager = FindObjectOfType<ItemManager>();
        if (!inventoryPanel) inventoryPanel = FindObjectOfType<InventoryPanel>();
    }

    public void OnPointerExit()
    {
        inventoryPanel.Tooltip.Hide();
    }

    public void OnPointerEnter()
    {
        var t = this.transform as RectTransform;
        inventoryPanel.Tooltip.Show(item, t);
    }

    public void SetItem(InventoryItem item)
    {
        this.item = item;
        if (item == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        this.gameObject.SetActive(true);
        if (item.Item.Icon)
        {
            this.itemIcon.sprite = item.Item.Icon;
        }
        this.lblAmount.gameObject.SetActive(item.Item.Stackable);
        this.lblAmount.text = item.Amount.ToString();
    }
}
