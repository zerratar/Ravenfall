using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickEventArgs : EventArgs
{
    public MouseClickEventArgs(params RaycastHit[] Hits)
    {
        Collection = Hits;
    }

    public RaycastHit[] Collection { get; }
}