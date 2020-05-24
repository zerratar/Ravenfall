using System;
using System.Linq;

namespace Shinobytes.Ravenfall.RavenNet.Models
{
    public class Player
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public int CombatLevel { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Destination { get; set; }
        public Appearance Appearance { get; set; }
    }
}