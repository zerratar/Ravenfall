using UnityEngine;
using UnityEngine.UI;

public class VendorItemRow : MonoBehaviour
{
    [SerializeField] private Image imgIcon;
    [SerializeField] private Image imgSelection;
    [SerializeField] private TMPro.TextMeshProUGUI lblName;
    [SerializeField] private TMPro.TextMeshProUGUI lblPrice;
    [SerializeField] private TMPro.TextMeshProUGUI lblAmount;

    public ServerItem Item { get; private set; }

    private NpcTradePanel npcTradePanel;

    public void Start()
    {
        HideSelection();
    }

    public void OnEnable()
    {
        if (!npcTradePanel)
            npcTradePanel = gameObject.GetComponentInParent<NpcTradePanel>();
    }

    public void OnClick()
    {
        npcTradePanel.OnVendorRowClicked(this);
    }

    public void ShowSelection()
    {
        imgSelection.enabled = true;
    }

    public void HideSelection()
    {
        imgSelection.enabled = false;
    }

    public void SetData(ServerItem item, int price, int amount)
    {
        Item = item;
        imgIcon.sprite = item.Icon;
        lblName.text = item.Name;
        lblPrice.text = price.ToString();
        lblAmount.text = amount.ToString();
    }
}
