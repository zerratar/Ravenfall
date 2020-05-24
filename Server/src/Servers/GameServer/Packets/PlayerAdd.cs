using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class PlayerAdd
    {
        public const short OpCode = 2;
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public int CombatLevel { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public Appearance Appearance { get; set; }
        internal static PlayerAdd Create(Player player, int combatLevel)
        {
            return new PlayerAdd
            {
                PlayerId = player.Id,
                Name = player.Name,
                CombatLevel = combatLevel,
                Position = player.Position,
                Destination = player.Destination,
                Appearance = player.Appearance
            };
        }
    }
}
