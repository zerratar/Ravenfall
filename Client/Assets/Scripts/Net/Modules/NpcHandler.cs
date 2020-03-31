using Shinobytes.Ravenfall.RavenNet.Models;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class NpcHandler : EntityHandler<Npc>
    {
        public NpcHandler() 
            : base((a, b) => a.Id == b.Id)
        {
        }

        public override string Name => "Npc Handler";
    }
}
