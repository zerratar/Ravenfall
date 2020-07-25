using Assets.Scripts.Extensions;
using Shinobytes.Ravenfall.RavenNet.Models;
using System.Collections;
using System.Linq;
using UnityEngine;

public class CharacterSelectionPanelUI : MonoBehaviour
{
    [SerializeField] private NetworkClient networkClient;
    [SerializeField] private GameObject selectionView;
    [SerializeField] private GameObject creationView;
    [SerializeField] private GameObject playerPreviewPoint;
    [SerializeField] private GameObject playerPreviewPrefab;

    [Header("Character Selection")]
    [SerializeField] private TMPro.TextMeshProUGUI lblCharacterName;
    [SerializeField] private CharacterDeleteConfirmationDialog characterDeleteDialog;
    [SerializeField] private UnityEngine.UI.Button btnExit;

    [SerializeField] private UnityEngine.UI.Button btnPlay;
    [SerializeField] private UnityEngine.UI.Button btnNewCharacter;
    [SerializeField] private CharacterCreation characterCreation;

    [Header("Character Creation")]
    [SerializeField] private UnityEngine.UI.Button btnBack;

    private CharacterSelectRow[] characterSelectionRows;

    private int playerCount = 0;
    private Player selectedPlayer;
    // Start is called before the first frame update
    void Start()
    {
        if (!networkClient) networkClient = FindObjectOfType<NetworkClient>();

        characterSelectionRows = selectionView.GetComponentsInChildren<CharacterSelectRow>();
        btnNewCharacter.interactable = true;

        SetupEvents();
        ResetView();
    }

    private void ResetView()
    {
        characterSelectionRows[0].Selected = true;
        SetPlayers(new Player[0]);
        OnPlayerSelected(null);
        HideCharacterCreation();
    }

    private void SetupEvents()
    {
        btnExit.onClick.AddListener(new UnityEngine.Events.UnityAction(OnExitClicked));
        btnPlay.onClick.AddListener(new UnityEngine.Events.UnityAction(OnPlayClicked));
        btnNewCharacter.onClick.AddListener(new UnityEngine.Events.UnityAction(OnNewCharClicked));
        btnBack.onClick.AddListener(new UnityEngine.Events.UnityAction(OnBackClicked));
    }

    private void OnBackClicked()
    {
        HideCharacterCreation();
    }

    private void OnNewCharClicked()
    {
        ShowCharacterCreation();
    }

    private void OnPlayClicked()
    {
        networkClient.SendSelectCharacter(selectedPlayer.Session.Name, selectedPlayer.Id);
    }

    private void OnExitClicked()
    {
    }

    public void ShowDeleteCharacterDialog(CharacterSelectRow row)
    {
        characterDeleteDialog.gameObject.SetActiveFast(true);
        characterDeleteDialog.CharacterRow = row;
    }

    public void HideDeleteCharacterDialog()
    {
        characterDeleteDialog.gameObject.SetActiveFast(false);
    }

    internal EntityEquipmentHandler GetPreviewCharacter()
    {
        return playerPreviewPoint.transform.GetChild(0).GetComponent<EntityEquipmentHandler>();
    }

    public void ShowCharacterCreation()
    {
        characterCreation.GenerateCharacter();
        creationView.SetActiveFast(true);
        selectionView.SetActiveFast(false);
    }

    public void HideCharacterCreation()
    {
        RemovePlayerPresentation();
        creationView.SetActiveFast(false);
        selectionView.SetActiveFast(true);
    }

    // Update is called once per frame
    void Update()
    {
        // hax, for now
        // as long as the list is empty, keep trying to update it
        if (!networkClient) return;
        if (playerCount != networkClient.CharacterHandler.Players.Length)
            SetPlayers(networkClient.CharacterHandler.Players);
    }

    internal void SetPlayers(Player[] players)
    {
        var selectedIndex = 0;
        for (int i = 0; i < characterSelectionRows.Length; i++)
        {
            var row = characterSelectionRows[i];
            if (row.Selected)
                selectedIndex = i;

            row.Player = i < players.Length ? players[i] : null;
        }

        playerCount = players.Length;

        OnPlayerSelected(characterSelectionRows[selectedIndex].Player);
    }

    internal void SelectCharacter(CharacterSelectRow characterSelectRow)
    {
        foreach (var row in characterSelectionRows)
        {
            row.Selected = false;
        }

        characterSelectRow.Selected = true;
        OnPlayerSelected(characterSelectRow.Player);
    }

    internal void ConfirmDeleteCharacter(CharacterSelectRow characterSelectRow)
    {
        ShowDeleteCharacterDialog(characterSelectRow);
    }

    internal void DeleteCharacter(Player player)
    {
        networkClient.SendDeleteCharacter(player.Id);
    }

    internal void CreateCharacter(Player player)
    {
        networkClient.SendCreateCharacter(player);
    }

    private void OnPlayerSelected(Player player)
    {
        selectedPlayer = player;

        RemovePlayerPresentation();

        btnNewCharacter.interactable = characterSelectionRows.Any(x => x.Selected && player == null);

        if (player == null)
        {
            return;
        }

        UpdatePlayerPresentation(player);
    }

    public void UpdatePlayerPresentation(Player player)
    {
        // Generate Player using player data
        // Add as child to characterPosition
        // set layer to CharacterSelection
        btnPlay.interactable = true;
        lblCharacterName.text = player.Name;
        StartCoroutine(CreatePreview(player));
    }

    private IEnumerator CreatePreview(Player player)
    {
        //yield return new WaitForEndOfFrame();
        //Instantiate(playerPreviewPrefab, playerPreviewPoint.transform).GetComponent<EntityEquipmentHandler>();
        playerPreviewPoint.transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForEndOfFrame();
        CreatePlayerPreview(player);
    }


    public void RemovePlayerPresentation()
    {
        btnPlay.interactable = false;

        if (playerPreviewPoint.transform.childCount == 0)
        {
            return;
        }

        playerPreviewPoint.transform.GetChild(0).gameObject.SetActive(false);

        if (!lblCharacterName) return;
        lblCharacterName.text = "";
        //SetActiveFast(playerPreview.gameObject, false);
    }

    private static void SetLayerRecursive(GameObject obj, int layer)
    {
        obj.layer = layer;
        for (var i = 0; i < obj.transform.childCount; ++i)
        {
            SetLayerRecursive(obj.transform.GetChild(i).gameObject, layer);
        }
    }

    private void CreatePlayerPreview(Player player)
    {
        if (playerPreviewPoint.transform.childCount == 0) return;
        var child = playerPreviewPoint.transform.GetChild(0);
        var preview = child.GetComponent<EntityEquipmentHandler>();
        if (!preview)
        {
            return;
        }

        preview.SetAppearance(player?.Appearance);
        SetLayerRecursive(preview.gameObject, preview.gameObject.layer);
    }
}
