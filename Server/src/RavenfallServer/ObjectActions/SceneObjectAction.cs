namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public abstract class SceneObjectAction
    {
        protected SceneObjectAction(int id, string name)
        {
            Id = id;
            Name = name;
        }
        
        public int Id { get; }
        public string Name { get; }
        public abstract bool Invoke(Player player, SceneObject obj, int parameterId);
    }
}
