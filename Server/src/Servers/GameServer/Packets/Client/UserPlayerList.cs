using Shinobytes.Ravenfall.RavenNet.Models;

namespace RavenfallServer.Packets
{
    public class UserPlayerList
    {
        public const short OpCode = 22;
        public int[] Id { get; set; }
        public string[] Name { get; set; }
        public int[] CombatLevel { get; set; }
        public Appearance[] Appearance { get; set; }

        public static UserPlayerList Create(Player[] players)
        {
            var ids = new int[players.Length];
            var names = new string[players.Length];
            var appearances = new Appearance[players.Length];
            var combatLevels = new int[players.Length];

            for (var i = 0; i < players.Length; ++i)
            {
                ids[i] = players[i].Id;
                names[i] = players[i].Name;
                appearances[i] = players[i].Appearance;
                combatLevels[i] = players[i].CombatLevel;
            }

            return new UserPlayerList
            {
                Id = ids,
                Name = names,
                CombatLevel = combatLevels,
                Appearance = appearances
            };
        }
    }
}
