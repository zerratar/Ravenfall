using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerManager playerManager;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        playerCamera.MouseClick += OnMouseClick;
    }

    private void OnMouseClick(object sender, MouseClickEventArgs e)
    {
        var myPlayer = playerManager.Me;
        if (!myPlayer) return;

        // depending on what is being clicked on
        // if its an object to interact with
        
        var worldObject = e.Object.transform.GetComponentInParent<NetworkObject>();
        if (worldObject)
        {
            myPlayer.MoveToAndInteractWith(worldObject);
        }
        else
        {
            myPlayer.MoveTo(e.Point);
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnDestroy()
    {
        playerCamera.MouseClick -= OnMouseClick;
    }
}