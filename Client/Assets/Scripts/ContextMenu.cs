using System.Collections.Generic;
using UnityEngine;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject contextMenuButtonPrefab;
    [SerializeField] private GameObject contextMenuItemContainer;
    [SerializeField] private TMPro.TextMeshProUGUI lblHeader;

    private readonly List<ContextMenuButton> buttons
        = new List<ContextMenuButton>();

    private RectTransform rectTransform;

    private void Start()
    {
        this.rectTransform = GetComponent<RectTransform>();
        Hide();
    }

    internal ContextMenu SetHeader(string header)
    {
        this.lblHeader.text = header;
        return this;
    }

    internal ContextMenu SetItems(params ContextMenuItem[] items)
    {
        foreach (var btn in buttons)
        {
            btn.gameObject.SetActive(false);
        }

        while (buttons.Count < items.Length)
        {
            var btnObj = Instantiate(contextMenuButtonPrefab, contextMenuItemContainer.transform);
            var btn = btnObj.GetComponent<ContextMenuButton>();
            buttons.Add(btn);
        }

        for (var i = 0; i < items.Length; ++i)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].Click = items[i].Click;
            buttons[i].Text = items[i].Text;
        }

        return this;
    }

    internal ContextMenu Show()
    {
        var mousePos = Input.mousePosition;
        this.gameObject.SetActive(true);
        this.rectTransform.position = mousePos;
        return this;
    }

    internal void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void OnMouseExit()
    {
        Debug.Log("ContextMenu::OnMouseExit");
        Hide();
    }
}
