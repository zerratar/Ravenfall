using Shinobytes.Ravenfall.RavenNet.Models;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectRow : MonoBehaviour
{
    [SerializeField] private Sprite defaultBackgroundImage;
    [SerializeField] private Sprite selectedBackgroundImage;

    [SerializeField] private UnityEngine.UI.Button btnSelect;
    [SerializeField] private UnityEngine.UI.Button btnDelete;

    [SerializeField] private TMPro.TextMeshProUGUI lblName;
    [SerializeField] private TMPro.TextMeshProUGUI lblLevel;

    [SerializeField] private CharacterSelectionPanelUI characterSelection;

    private Player player;

    public bool Selected { get; set; }
    public Player Player
    {
        get => player; set
        {
            player = value;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (player == null)
        {
            lblName.text = "Empty";
            lblLevel.text = "";
            return;
        }

        lblName.text = player.Name;
        lblLevel.text = $"lv. {player.CombatLevel}";
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!characterSelection) characterSelection = FindObjectOfType<CharacterSelectionPanelUI>();

        btnSelect.onClick.AddListener(new UnityEngine.Events.UnityAction(OnSelectClicked));
        btnDelete.onClick.AddListener(new UnityEngine.Events.UnityAction(OnDeleteClicked));
    }

    private void OnSelectClicked()
    {
        characterSelection.SelectCharacter(this);
    }

    private void OnDeleteClicked()
    {
        characterSelection.ConfirmDeleteCharacter(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!btnSelect || !btnDelete) return;

        SetActiveFast(btnDelete.gameObject, Player != null && Selected);
        SetSpriteFast(btnSelect.image, Selected ? selectedBackgroundImage : defaultBackgroundImage);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetSpriteFast(Image img, Sprite sprite)
    {
        if (img.sprite.name == sprite.name)
        {
            return;
        }

        img.sprite = sprite;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetActiveFast(GameObject obj, bool state)
    {
        if (obj.activeSelf == state)
        {
            return;
        }

        obj.SetActive(state);
    }
}