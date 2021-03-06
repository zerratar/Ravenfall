﻿using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class PlayerInventoryHandler : INetworkPacketHandler<PlayerInventory>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public PlayerInventoryHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(PlayerInventory data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            UnityEngine.Debug.Log("Player Inventory Updated");
            var playerHandler = moduleManager.GetModule<PlayerHandler>();
            playerHandler.SetPlayerInventory(data.PlayerId, data.Coins, data.ItemId, data.Amount);
        }
    }
}
