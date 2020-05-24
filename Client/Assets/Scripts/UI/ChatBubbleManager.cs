using System.Collections.Generic;
using UnityEngine;

public class ChatBubbleManager : MonoBehaviour
{
    [SerializeField] private GameObject chatBubblePrefab;
    [SerializeField] private GameObject chatBubbleContainer;
    [SerializeField] private float messageDuration = 2f;

    private readonly Dictionary<int, ChatBubble> chatBubbles
        = new Dictionary<int, ChatBubble>();

    private void Update()
    {
    }

    public void AddChatBubble(NetworkPlayer player, string message)
    {
        if (!chatBubbles.TryGetValue(player.Id, out var bubble))
        {
            var chatBubbleObj = Instantiate(chatBubblePrefab, chatBubbleContainer.transform);
            bubble = chatBubbleObj.GetComponent<ChatBubble>();
            chatBubbles[player.Id] = bubble;
        }

        bubble.ShowMessage(player, message, messageDuration);
    }
}
