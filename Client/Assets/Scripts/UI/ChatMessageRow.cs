using UnityEngine;

public class ChatMessageRow : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI lblMessage;

    public void SetMessage(string sender, string message, string senderColor = "#ffff00", bool playerMessage = true)
    {
        lblMessage.text = !string.IsNullOrEmpty(sender)
            ? playerMessage ? $"<color={senderColor}>{sender}: <color=#ffffff>{message}" : $"<color={senderColor}>[{sender}]: <color=#ffffff>{message}"
            : message;
    }
}
