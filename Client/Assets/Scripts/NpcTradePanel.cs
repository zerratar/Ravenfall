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
    private bool vendorActive;
    void Start()
    {
        if (!itemManager)
            itemManager = FindObjectOfType<ItemManager>();

        if (!playerManager)
            playerManager = FindObjectOfType<PlayerManager>();

        if (!networkClient)
            networkClient = FindObjectOfType<NetworkClient>();

        gameObject.SetActive(false);
        btnBuy.interactable = CanBuySelectedItem();
        btnSell.interactable = CanSellSelectedItem();
    }

    internal void OnVendorRowClicked(VendorItemRow vendorItemRow)
    {
        if (selectedRow)
        {
            selectedRow.HideSelection();
        }

        selectedRow = vendorItemRow;
        selectedRow.ShowSelection();

        btnBuy.interactable = CanBuySelectedItem();
        btnSell.interactable = CanSellSelectedItem();
    }

    internal void Show(int npcId, string npcName, string shopName, int[] itemId, int[] itemPrice, int[] itemStock)
    {
        vendorActive = true;

        // ensure we update the can buy / can sell states
        // by clearing out previous state. But keep the last ID so we can re-select it.
        var previousSelection = ClearSelection();

        if (!itemManager)
            itemManager = FindObjectOfType<ItemManager>();

        targetNpcId = npcId;

        if (!string.IsNullOrEmpty(shopName))
            lblShopName.text = shopName;

        gameObject.SetActive(true);

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

                if (previousSelection == itemRows[i].Item.Id)
                {
                    selectedRow = itemRows[i];
                    selectedRow.ShowSelection();
                }
            }
        }

        btnBuy.interactable = CanBuySelectedItem();
        btnSell.interactable = CanSellSelectedItem();
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
        vendorActive = false;

        ClearSelection();

        this.gameObject.SetActive(false);
    }

    private int ClearSelection()
    {
        var selectedId = -1;
        if (selectedRow)
        {
            selectedId = selectedRow.Item.Id;
            selectedRow.HideSelection();
            selectedRow = null;
            btnBuy.interactable = CanBuySelectedItem();
            btnSell.interactable = CanSellSelectedItem();
        }
        return selectedId;
    }

    private bool CanBuySelectedItem()
    {
        return vendorActive && selectedRow && selectedRow.Price <= playerManager.Me.Inventory.Coins && selectedRow.Amount > 0;
    }

    private bool CanSellSelectedItem()
    {
        return vendorActive && selectedRow && playerManager.Me.Inventory.HasItem(selectedRow.Item);
    }
}
