using System;
using Shinobytes.Ravenfall.RavenNet.Models;

internal abstract class EntityTick
{
    internal abstract bool Invoke(TimeSpan deltaTime);
}
internal class EntityTick<TObject> : EntityTick where TObject : Entity
{
    public EntityTick(Player player, TObject obj, EntityTickHandler<TObject> handleObjectTick)
    {
        this.totalTime = TimeSpan.Zero;
        this.Player = player;
        this.Object = obj;
        this.Action = handleObjectTick;
    }

    private TimeSpan totalTime;

    public Player Player { get; }
    public TObject Object { get; }
    public EntityTickHandler<TObject> Action { get; }

    internal override bool Invoke(TimeSpan deltaTime)
    {
        totalTime = totalTime.Add(deltaTime);
        return Action.Invoke(Player, Object, totalTime, deltaTime);
    }
}