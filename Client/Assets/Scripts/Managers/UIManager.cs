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

    [SerializeField] private ChatPanel chatPanel;
    [SerializeField] private ContextMenu contextMenu;

    private int uiMask;

    public ContextMenu ContextMenu => contextMenu;

    private void Start()
    {
        uiMask = LayerMask.NameToLayer("UI");
    }

    public bool HasFocus
    {
        get
        {
            return chatPanel.HasFocus; // || ;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!networkClient) return;
        if (!subSceneManager) return;

        SetActiveFast(bottomBarUI.gameObject, subSceneManager.SubSceneIndex == 2);
        SetActiveFast(charSelectionUI.gameObject, subSceneManager.SubSceneIndex == 1);
        SetActiveFast(loginPanel.gameObject, subSceneManager.SubSceneIndex == 0);
    }

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