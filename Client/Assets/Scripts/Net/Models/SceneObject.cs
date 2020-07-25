namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class SceneObject
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public Vector3 Position { get; set; }
        public bool Static { get; set; }
    }
}
