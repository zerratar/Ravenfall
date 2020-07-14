using Shinobytes.Ravenfall.RavenNet.Models;
using System;

public class AfterActionEventArgs : EventArgs
{
    public Player Player { get; }
    public Shinobytes.Ravenfall.RavenNet.Models.WorldObject Object { get; }
    public AfterActionEventArgs(Player player, Shinobytes.Ravenfall.RavenNet.Models.WorldObject obj)
    {
        this.Player = player;
        this.Object = obj;
    }
}
