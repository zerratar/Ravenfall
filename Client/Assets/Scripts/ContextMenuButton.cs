using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMPro.TextMeshProUGUI lblHeader;
    private ContextMenu contextMenu;

    public Action Click { get; set; }
    public string Text
    {
        set => lblHeader.text = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        contextMenu = GetComponentInParent<ContextMenu>();
        button.onClick.AddListener(new UnityEngine.Events.UnityAction(OnClick));
    }

    private void OnClick()
    {
        if (Click != null)
        {
            Click();
        }
        
        contextMenu.Hide();
    }
}
