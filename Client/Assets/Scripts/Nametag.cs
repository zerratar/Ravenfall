using UnityEngine;

public class Nametag : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI lblName;
    [SerializeField] private float offsetY = 1.5f;
    [SerializeField] private PlayerCamera playerCamera;

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
    }

    public void SetPlayer(NetworkPlayer player)
    {
        if (!lblName) return;
        lblName.text = player.name;
        lblName.faceColor = player.IsMe ? Color.green : Color.white;
        this.player = player;
    }
}
