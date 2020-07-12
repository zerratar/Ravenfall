using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class PlayerHealthUpdated : EntityUpdated<Player>
    {
        public PlayerHealthUpdated(
            Player entity, int health, int maxHealth, int delta)
            : base(entity)
        {
            Health = health;
            MaxHealth = maxHealth;
            Delta = delta;
        }

        public int Health { get; }
        public int MaxHealth { get; }
        public int Delta { get; }
    }
}
