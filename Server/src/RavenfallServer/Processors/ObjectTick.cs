using System;
using Shinobytes.Ravenfall.RavenNet.Models;

internal abstract class ObjectTick
{
    internal abstract bool Invoke(TimeSpan deltaTime);
}
internal class ObjectTick<TObject> : ObjectTick where TObject : SceneObject
{
    public ObjectTick(Player player, TObject obj, ObjectTickHandler<TObject> handleObjectTick)
    {
        this.totalTime = TimeSpan.Zero;
        this.Player = player;
        this.Object = obj;
        this.Action = handleObjectTick;
    }

    private TimeSpan totalTime;

    public Player Player { get; }
    public TObject Object { get; }
    public ObjectTickHandler<TObject> Action { get; }

    internal override bool Invoke(TimeSpan deltaTime)
    {
        totalTime = totalTime.Add(deltaTime);
        return Action.Invoke(Player, Object, totalTime, deltaTime);
    }
}