using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [SerializeField] private InventoryItemSlot[] inventoryItemSlots;
    [SerializeField] private InventoryItemTooltip tooltip;
    [SerializeField] private TMPro.TextMeshProUGUI lblCoins;

    public InventoryItemTooltip Tooltip => tooltip;

    // Start is called before the first frame update
    void Start()
    {
        this.inventoryItemSlots = GetComponentsInChildren<InventoryItemSlot>();
        this.SetInventoryItems();

        if (this.gameObject.activeSelf)
            this.gameObject.SetActive(false);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    internal void SetCoins(long amount)
    {
        if (lblCoins)
        {
            lblCoins.text = amount.ToString();
        }
    }

    public void SetInventoryItems(params InventoryItem[] items)
    {
        Debug.Log("InventoryPanel::SetInventoryItems");
        for (var i = 0; i < inventoryItemSlots.Length; ++i)
        {
            if (i < items.Length)
            {
                inventoryItemSlots[i].SetItem(items[i]);
            }
            else
            {
                inventoryItemSlots[i].SetItem(null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
