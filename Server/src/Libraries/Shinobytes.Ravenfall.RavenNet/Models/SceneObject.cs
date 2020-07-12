using System;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class SceneObject : WorldEntity
    {
        public int ObjectId { get; set; }
        public int DisplayObjectId { get; set; }
        public Vector3 Position { get; set; }
        public decimal Experience { get; set; }
        public int InteractItemType { get; set; }
        public int RespawnMilliseconds { get; set; }
        public bool Static { get; set; }
    }
    public class SceneObjectItemDrops
    {
        public int ObjectId { get; set; }
        public ObjectItemDrop[] Drops { get; set; }
    }

    public class ObjectItemDrop
    {
        public int ItemId { get; set; }
        public float DropChance { get; set; }
    }

}
