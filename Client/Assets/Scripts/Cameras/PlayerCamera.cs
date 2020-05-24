using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

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


        var mousePos = Input.mousePosition;
        var ray = camera.ScreenPointToRay(mousePos);

        if (Input.GetMouseButtonDown(0))
        {
            if (uiManager.IsMouseOverUI())
            {
                return;
            }

            var hits = Physics.RaycastAll(ray, clickMaxDistance);

            if (hits.Length > 0)
            {
                MouseClick?.Invoke(this, new MouseClickEventArgs(hits));
            }
        }
        else
        {
            var hits = Physics.RaycastAll(ray, clickMaxDistance);
            var wasHit = false;

            if (!uiManager.IsMouseOverUI())
            {
                foreach (var hitInfo in hits)
                {
                    var name = hitInfo.collider.name;
                    var obj = hitInfo.collider.gameObject;
                    if (name.IndexOf("::", StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        if (lastMouseOver && lastMouseOver.GetInstanceID() == obj.GetInstanceID())
                        {
                            return;
                        }

                        wasHit = true;
                        MouseEnter?.Invoke(this, new MouseClickEventArgs(hitInfo));
                        lastMouseOver = obj;
                        break;
                    }
                }
            }

            if (!wasHit && lastMouseOver)
            {
                MouseExit?.Invoke(this, new MouseClickEventArgs());
                lastMouseOver = null;
            }
        }
    }
}
