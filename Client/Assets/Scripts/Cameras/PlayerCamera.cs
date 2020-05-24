using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private UIManager uiManager;

    private float clickMaxDistance = 5000f;
    private GameObject lastMouseOver;

    public event EventHandler<MouseClickEventArgs> MouseClick;
    public event EventHandler<MouseClickEventArgs> MouseEnter;
    public event EventHandler<MouseClickEventArgs> MouseExit;

    // Start is called before the first frame update
    void Start()
    {
        if (!uiManager) uiManager = FindObjectOfType<UIManager>();
        if (!camera) camera = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseButtonClick(0);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            HandleMouseButtonClick(1);
        }
        else
        {
            HandleMouseOver();
        }
    }

    private void HandleMouseButtonClick(int mouseButton)
    {
        if (uiManager.IsMouseOverUI())
        {
            return;
        }

        var mousePos = Input.mousePosition;
        var ray = camera.ScreenPointToRay(mousePos);
        var hits = Physics.RaycastAll(ray, clickMaxDistance);

        if (hits.Length > 0)
        {
            MouseClick?.Invoke(this, new MouseClickEventArgs(mouseButton, hits));
        }
    }

    private void HandleMouseOver()
    {
        var mousePos = Input.mousePosition;
        var ray = camera.ScreenPointToRay(mousePos);
        var hits = Physics.RaycastAll(ray, clickMaxDistance);
        var wasHit = false;

        if (!uiManager.IsMouseOverUI())
        {
            foreach (var hitInfo in hits)
            {
                var obj = hitInfo.collider.gameObject;
                if (lastMouseOver && lastMouseOver.GetInstanceID() == obj.GetInstanceID())
                {
                    return;
                }

                var name = hitInfo.collider.name;
                if (name.IndexOf("::", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    wasHit = true;
                    MouseEnter?.Invoke(this, new MouseClickEventArgs(-1, hitInfo));
                    lastMouseOver = obj;
                    break;
                }
            }
        }

        if (!wasHit && lastMouseOver)
        {
            OnMouseLeave();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseLeave()
    {
        MouseExit?.Invoke(this, new MouseClickEventArgs(-1));
        lastMouseOver = null;
    }
}
