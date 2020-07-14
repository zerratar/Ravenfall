using System;
using Shinobytes.Ravenfall.RavenNet.Models;

namespace GameServer.Processors
{
    internal abstract class EntityTick
    {
        internal abstract bool Invoke(TimeSpan deltaTime);
    }

    internal class EntityTick<TObject> : EntityTick where TObject : Entity
    {
        public EntityTick(Player player, TObject obj, EntityTickHandler<TObject> handleObjectTick)
        {
            totalTime = TimeSpan.Zero;
            Player = player;
            Object = obj;
            Action = handleObjectTick;
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
}