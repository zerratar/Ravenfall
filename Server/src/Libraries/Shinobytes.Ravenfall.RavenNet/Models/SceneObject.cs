using System;

namespace Shinobytes.Ravenfall.RavenNet.Models
{

    public class SceneObject
    {
        public int Id { get; set; }
        public int ObjectId { get; set; }
        public int DisplayObjectId { get; set; }
        public Vector3 Position { get; set; }
        public decimal Experience { get; set; }
        public int InteractItemType { get; set; }
        public int RespawnMilliseconds { get; set; }
    }
}
