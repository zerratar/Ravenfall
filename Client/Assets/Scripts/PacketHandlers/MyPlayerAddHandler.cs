using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class MyPlayerAddHandler : INetworkPacketHandler<MyPlayerAdd>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public MyPlayerAddHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(MyPlayerAdd data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var hasAppearance = data.Appearance != null;
            UnityEngine.Debug.Log("Player: " + data.Name + ", received from server. POS: " + data.Position + ", has appearance: " + hasAppearance);

            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            var player = new Player()
            {
                Id = data.PlayerId,
                IsMe = true,
                Name = data.Name,
                Position = data.Position,
                Appearance = data.Appearance,
            };

            playerHandler.Add(player);
            playerHandler.PlayerStatsUpdate(player.Id, data.Experience, data.EffectiveLevel);
            playerHandler.SetPlayerInventory(player.Id, data.Coins, data.InventoryItemId, data.InventoryItemAmount);

        }
    }
}
