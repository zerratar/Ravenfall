using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Serializers;
using Shinobytes.Ravenfall.RavenNet.Server;
using System;

namespace Shinobytes.Ravenfall.ZoneServer
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
            ioc.RegisterShared<IZone, Zone>();
            ioc.RegisterShared<IMessageBus, MessageBus>();
            ioc.RegisterShared<IProcessStatusProvider, ProcessStatusProvider>();
            ioc.RegisterShared<IKernel, Kernel>();
            return ioc;
        }

        static void Main(string[] args)
        {
            Console.Title = "Ravenfall - Zone Server";

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
