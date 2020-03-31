using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera camera;

    private float clickMaxDistance = 5000f;

    public event EventHandler<MouseClickEventArgs> MouseClick;

    // Start is called before the first frame update
    void Start()
    {
        if (!camera) camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            var ray = camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out var hitInfo, clickMaxDistance))
            {
                MouseClick?.Invoke(this, new MouseClickEventArgs(hitInfo.collider.gameObject, hitInfo.point));
            }
        }
    }
}
