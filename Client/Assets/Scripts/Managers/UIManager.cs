using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private NetworkClient networkClient;
    [SerializeField] private SubSceneManager subSceneManager;
    [SerializeField] private LoginPanelUI loginPanel;
    [SerializeField] private IngameBottomBarUI bottomBarUI;
    [SerializeField] private CharacterSelectionPanelUI charSelectionUI;

    [SerializeField] private NpcTradePanel npcTradePanel;

    [SerializeField] private ChatPanel chatPanel;
    [SerializeField] private ContextMenu contextMenu;
    [SerializeField] private InventoryPanel inventoryPanel;
    [SerializeField] private NpcManager npcManager;

    [SerializeField] private TwitchConfigurationDialogUI twitchConfigDialogUI;

    private int uiMask;

    public ChatPanel ChatPanel => chatPanel;
    public ContextMenu ContextMenu => contextMenu;
    public InventoryPanel InventoryPanel => inventoryPanel;

    internal void ShowTwitchConfigurationDialog(Settings settings)
    {
        if (!twitchConfigDialogUI) return;
        twitchConfigDialogUI.Show(settings);
    }

    private void Start()
    {
        uiMask = LayerMask.NameToLayer("UI");
        if (!npcManager) npcManager = FindObjectOfType<NpcManager>();
    }

    public bool HasFocus
    {
        get
        {
            return chatPanel.HasFocus; // || ;
        }
    }

    internal void OnNpcTradeWindowOpen(
        Player entity,
        int npcServerId,
        string shopName,
        int[] itemId,
        int[] itemPrice,
        int[] itemStock)
    {
        var npc = npcManager.GetNpcByServerId(npcServerId);
        if (!npc) return;

        inventoryPanel.gameObject.SetActive(true);
        npcTradePanel.Show(npc.ServerId, npc.name, shopName, itemId, itemPrice, itemStock);
    }

    // Update is called once per frame
    void Update()
    {
        if (!networkClient) return;
        if (!subSceneManager) return;

        SetActiveFast(bottomBarUI.gameObject, subSceneManager.SubSceneIndex == 2);
        SetActiveFast(charSelectionUI.gameObject, subSceneManager.SubSceneIndex == 1);
        SetActiveFast(loginPanel.gameObject, subSceneManager.SubSceneIndex == 0);

        if (!HasFocus)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }
        }
    }

    public void ToggleInventory()
    {
        inventoryPanel.gameObject.SetActive(!inventoryPanel.gameObject.activeSelf);
    }

    internal void OnTwitchConnectionLost()
    {
    }

    internal void OnTwitchConnectionEstablished()
    {
    }

    public void ToggleMenu() { }
    public void ToggleStats() { }
    public void TogglePlayerInfo() { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetActiveFast(GameObject obj, bool state)
    {
        if (!obj || obj.activeSelf == state) return;
        obj.SetActive(state);
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsMouseOverUI()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    ///Returns 'true' if we touched or hovering on Unity UI element.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        return eventSystemRaysastResults.Any(x => x.gameObject.layer == uiMask);
    }

    ///Gets all event systen raycast results of current mouse or touch position.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static List<RaycastResult> GetEventSystemRaycastResults()
    {
        var raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        }, raysastResults);
        return raysastResults;
    }

}