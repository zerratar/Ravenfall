using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Serializers;
using Shinobytes.Ravenfall.RavenNet.Server;
using System;

namespace Shinobytes.Ravenfall.GameServer
{

    class Program
    {
        private static IoC RegisterServices()
        {
            var ioc = new IoC();
            ioc.RegisterCustomShared<IoC>(() => ioc);
            ioc.RegisterShared<ILogger, ConsoleLogger>();
            ioc.RegisterShared<IBinarySerializer, BinarySerializer>();
            ioc.RegisterShared<INetworkPacketTypeRegistry, NetworkPacketTypeRegistry>();
            ioc.RegisterShared<INetworkPacketSerializer, NetworkPacketSerializer>();
            ioc.RegisterShared<IRavenServer, Server>(); // so we can reference this from packet handlers
            ioc.RegisterShared<INetworkPacketController, NetworkPacketController>();
            ioc.RegisterShared<IMessageBus, MessageBus>();
            ioc.RegisterShared<IZoneServerProvider, ZoneServerProvider>();
            return ioc;
        }

        static void Main(string[] args)
        {
            Console.Title = "Ravenfall - Game Server";

            var ioc = RegisterServices();
            using (var server = ioc.Resolve<IRavenServer>().Start())
            {
                while (true)
                {
                    var str = Console.ReadLine();
                    switch (str)
                    {
                        case "exit":
                        case "quit":
                        case "q":
                            return;
                    }
                }
            }
        }
    }
}
