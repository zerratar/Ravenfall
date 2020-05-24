using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ChatPanel : MonoBehaviour
{
    [SerializeField] private NetworkClient networkClient;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private ChatBubbleManager chatBubbleManager;

    [SerializeField] private GameObject chatMessageContainer;
    [SerializeField] private GameObject chatMessageRowPrefab;
    [SerializeField] private TMPro.TMP_InputField inputChatMessage;
    [SerializeField] private ScrollRect chatScrollRect;

    public bool HasFocus { get; internal set; }

    private int currentChannelId = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (!networkClient) networkClient = FindObjectOfType<NetworkClient>();
        if (!playerManager) playerManager = FindObjectOfType<PlayerManager>();
        if (!chatBubbleManager) chatBubbleManager = FindObjectOfType<ChatBubbleManager>();
        inputChatMessage.onSubmit.AddListener(new UnityEngine.Events.UnityAction<string>(SendChatMessage));
        inputChatMessage.onSelect.AddListener(new UnityEngine.Events.UnityAction<string>(OnInputSelect));
        inputChatMessage.onDeselect.AddListener(new UnityEngine.Events.UnityAction<string>(OnInputDeselect));
    }

    private void Update()
    {
        var message = networkClient.ChatMessageHandler.GetNextMessage();
        if (message != null)
        {
            var isMe = playerManager.Me.Id == message.PlayerId;
            var player = playerManager.GetPlayerById(message.PlayerId);
            AddMessage(player, message.Message, isMe ? "#00ff00" : "#ffff00");
        }
    }

    private void OnInputDeselect(string _)
    {
        HasFocus = false;
    }

    private void OnInputSelect(string _)
    {
        HasFocus = true;
    }

    private void SendChatMessage(string message)
    {
        if (string.IsNullOrEmpty(message)) return;
        networkClient.SendChatMessage(currentChannelId, message);
        inputChatMessage.text = "";
    }

    private void AddMessage(NetworkPlayer player, string message, string senderColor = "#ffff00")
    {
        Instantiate(chatMessageRowPrefab, chatMessageContainer.transform)
            .GetComponent<ChatMessageRow>()
            .SetMessage(player.name, message, senderColor);

        chatBubbleManager.AddChatBubble(player, message);

        StartCoroutine(ScrollToBottom());
    }

    private IEnumerator ScrollToBottom()
    {
        if (chatScrollRect.verticalNormalizedPosition > 0.3f)
        {
            yield break;
        }

        for (var i = 0; i < 5; ++i)
        {
            yield return new WaitForEndOfFrame();
            chatScrollRect.verticalNormalizedPosition = 0f;
        }
    }
}
