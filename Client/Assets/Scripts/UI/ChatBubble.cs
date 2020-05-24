using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI lblMessage;
    [SerializeField] private float offsetY = 2.75f;
    [SerializeField] private PlayerCamera playerCamera;

    private float durationTimer = 0f;

    private NetworkPlayer player;

    private void Start()
    {
        if (!playerCamera) playerCamera = GameObject.FindObjectOfType<PlayerCamera>();
    }

    private void Update()
    {
        if (!player) return;
        if (!playerCamera) playerCamera = GameObject.FindObjectOfType<PlayerCamera>();
        if (!playerCamera) return;
        transform.position = player.transform.position + (Vector3.up * offsetY);
        transform.LookAt(playerCamera.transform);

        durationTimer -= Time.deltaTime;
        if (durationTimer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void ShowMessage(NetworkPlayer player, string message, float duration)
    {
        if (!lblMessage) return;
        gameObject.SetActive(true);
        lblMessage.text = message;
        this.player = player;
        durationTimer = duration;
    }
}
