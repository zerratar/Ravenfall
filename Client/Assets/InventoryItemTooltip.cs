using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemTooltip : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI lblName;
    //[SerializeField] private float offsetX = -50;
    [SerializeField] private float offsetY = -50;

    private int showCounter;
    private float posX;

    // Start is called before the first frame update
    void Start()
    {
        this.posX = this.transform.position.x;
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
    }

    internal void Show(InventoryItem item, RectTransform source)
    {
        ++showCounter;
        gameObject.SetActive(true);
        transform.position = new UnityEngine.Vector3(posX, source.position.y + offsetY, 0);
        lblName.text = item.Item.Name;
    }

    internal void Hide()
    {
        --showCounter;
        if (showCounter <= 0)
        {
            showCounter = 0;
            gameObject.SetActive(false);
        }
    }
}
