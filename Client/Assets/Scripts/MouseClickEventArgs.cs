using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseClickEventArgs : EventArgs
{
    public MouseClickEventArgs(int mouseButton, params RaycastHit[] Hits)
    {
        MouseButton = (MouseButton)mouseButton;
        Collection = Hits;
    }

    public MouseButton MouseButton { get; }
    public RaycastHit[] Collection { get; }

    public NetworkPlayer GetNetworkPlayer()
    {
        return Collection
            .Select(x => x.collider.GetComponentInParent<NetworkPlayer>())
            .FirstOrDefault(x => x != null);
    }

    public NetworkObject GetNetworkObject()
    {
        return Collection
            .Select(x => x.collider.GetComponentInParent<NetworkObject>())
            .FirstOrDefault(x => x != null);
    }

    public TerrainHitInfo GetTerrain()
    {
        return Collection
            .Select(x => new TerrainHitInfo(x.collider.GetComponent<Terrain>(), x.point))
            .FirstOrDefault(x => x.Terrain != null);
    }
}
public enum MouseButton
{
    Left,
    Right
}

public struct TerrainHitInfo
{
    public TerrainHitInfo(Terrain terrain, Vector3 point)
    {
        Terrain = terrain;
        Point = point;
    }

    public Terrain Terrain { get; }
    public Vector3 Point { get; }
}