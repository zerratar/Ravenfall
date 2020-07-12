using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcTradePanel : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI lblShopName;
    [SerializeField] private Transform itemsPanel;
    [SerializeField] private GameObject vendorItemRowPrefab;
    [SerializeField] private ItemManager itemManager;

    [SerializeField] private UnityEngine.UI.Button btnBuy;
    [SerializeField] private UnityEngine.UI.Button btnSell;

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private NetworkClient networkClient;

    private List<VendorItemRow> itemRows = new List<VendorItemRow>();
    private VendorItemRow selectedRow;
    private int targetNpcId;

    void Start()
    {
        if (!itemManager)
            itemManager = FindObjectOfType<ItemManager>();

        if (!playerManager)
            playerManager = FindObjectOfType<PlayerManager>();

        if (!networkClient)
            networkClient = FindObjectOfType<NetworkClient>();

        gameObject.SetActive(false);
        btnBuy.interactable = false;
        btnSell.interactable = false;
    }

    internal void OnVendorRowClicked(VendorItemRow vendorItemRow)
    {
        if (selectedRow)
        {
            selectedRow.HideSelection();
        }

        selectedRow = vendorItemRow;
        selectedRow.ShowSelection();

        btnBuy.interactable = true;
        btnSell.interactable = playerManager.Me.Inventory.HasItem(vendorItemRow.Item);
    }

    internal void Show(int npcId, string npcName, string shopName, int[] itemId, int[] itemPrice, int[] itemStock)
    {
        if (!itemManager)
            itemManager = FindObjectOfType<ItemManager>();

        targetNpcId = npcId;
        
        if (!string.IsNullOrEmpty(shopName))
            this.lblShopName.text = shopName;

        this.gameObject.SetActive(true);

        while (itemRows.Count < itemId.Length)
        {
            var itemRow = Instantiate(vendorItemRowPrefab, itemsPanel).GetComponent<VendorItemRow>();
            itemRows.Add(itemRow);
        }

        for (var i = 0; i < itemRows.Count; ++i)
        {
            var active = i < itemId.Length;
            itemRows[i].gameObject.SetActive(active);
            if (active)
            {
                var item = itemManager.GetItemById(itemId[i]);
                itemRows[i].SetData(item, itemPrice[i], itemStock[i]);
            }
        }
    }

    public void SellSelectedItem()
    {
        if (!selectedRow) return;
        networkClient.SendSellItem(targetNpcId, selectedRow.Item.Id, 1);
    }

    public void BuySelectedItem()
    {
        if (!selectedRow) return;
        networkClient.SendBuyItem(targetNpcId, selectedRow.Item.Id, 1);       
    }

    public void Hide()
    {
        if (selectedRow)
        {
            selectedRow.HideSelection();
            selectedRow = null;
            btnBuy.interactable = false;
            btnSell.interactable = false;
        }

        this.gameObject.SetActive(false);
    }
}
