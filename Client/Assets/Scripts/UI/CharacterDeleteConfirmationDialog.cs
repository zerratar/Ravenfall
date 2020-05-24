using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDeleteConfirmationDialog : MonoBehaviour
{
    [SerializeField] private CharacterSelectionPanelUI characterSelectionUI;
    [SerializeField] private UnityEngine.UI.Button btnConfirm;
    [SerializeField] private UnityEngine.UI.Button btnCancel;
    public CharacterSelectRow CharacterRow { get; internal set; }

    // Start is called before the first frame update
    void Start()
    {
        if (!characterSelectionUI) characterSelectionUI = FindObjectOfType<CharacterSelectionPanelUI>();
        btnConfirm.onClick.AddListener(new UnityEngine.Events.UnityAction(OnConfirmClicked));
        btnCancel.onClick.AddListener(new UnityEngine.Events.UnityAction(OnCancelClicked));
    }

    private void OnCancelClicked()
    {
        CharacterRow = null;
        characterSelectionUI.HideDeleteCharacterDialog();
    }

    private void OnConfirmClicked()
    {
        characterSelectionUI.DeleteCharacter(CharacterRow.Player);
        characterSelectionUI.HideDeleteCharacterDialog();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
