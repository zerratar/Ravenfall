using System;
using UnityEngine;

public class MouseClickEventArgs : EventArgs
{
    public MouseClickEventArgs(GameObject obj, Vector3 destination)
    {
        Object = obj;
        Point = destination;
    }

    public GameObject Object { get; }
    public Vector3 Point { get; }
}
