using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;

namespace Assets.Scripts.PacketHandlers
{
    public class UserPlayerListHandler : INetworkPacketHandler<UserPlayerList>
    {
        private readonly ILogger logger;
        private readonly IModuleManager moduleManager;

        public UserPlayerListHandler(ILogger logger, IModuleManager moduleManager)
        {
            this.logger = logger;
            this.moduleManager = moduleManager;
        }

        public void Handle(UserPlayerList data, IRavenNetworkConnection connection, SendOption sendOption)
        {
            var players = data.GetPlayers();
            UnityEngine.Debug.Log("UserPlayerList - (Player Count:" + players.Length + "), received from server.");
            var loginHandler = this.moduleManager.GetModule<CharacterHandler>();
            loginHandler.SetCharacterList(players);
        }
    }
}
