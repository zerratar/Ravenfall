using UnityEngine;

public class ChatMessageRow : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI lblMessage;

    public void SetMessage(string sender, string message, string senderColor = "#ffff00")
    {
        lblMessage.text = $"<color={senderColor}>[{sender}]</color> {message}";
    }
}
