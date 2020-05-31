using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        this.gameObject.SetActive(true);

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

        StartCoroutine(Refresh());

        return this;
    }

    private IEnumerator Refresh()
    {
        // wtf?
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTransform);
        for (var i = 0; i < 3; ++i)
        {
            yield return new WaitForEndOfFrame();
            LayoutRebuilder.ForceRebuildLayoutImmediate(this.rectTransform);
        }
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
