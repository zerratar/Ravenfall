using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class IoCContainer
    {
        public static IoC Instance { get; } = CreateContainer();

        private static IoC CreateContainer()
        {
            var ioc = new IoC();
            return RegisterServices(ioc);
        }

        private static IoC RegisterServices(IoC ioc)
        {
            ioc.RegisterCustomShared<IoC>(() => ioc);
            ioc.RegisterShared<Shinobytes.Ravenfall.RavenNet.Core.ILogger, ConsoleLogger>();
            ioc.RegisterShared<IBinarySerializer, BinarySerializer>();
            ioc.RegisterShared<INetworkPacketTypeRegistry, NetworkPacketTypeRegistry>();
            ioc.RegisterShared<INetworkPacketSerializer, NetworkPacketSerializer>();
            ioc.RegisterShared<INetworkPacketController, NetworkPacketController>();
            ioc.RegisterShared<IMessageBus, MessageBus>();
            ioc.RegisterShared<IModuleManager, ModuleManager>();

            ioc.RegisterShared<IRavenClient, RavenClient>(); // so we can reference this from packet handlers

            // Twitch stuff
            ioc.RegisterShared<ITwitchCommandController, TwitchCommandController>();


            return ioc;
        }
    }
}
