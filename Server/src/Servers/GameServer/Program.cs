using GameServer.Repositories;
using RavenfallServer.Network;
using RavenfallServer.Providers;
using RavenfallServer.Services;
using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Serializers;
using Shinobytes.Ravenfall.RavenNet.Server;
using System;
using System.Linq;
using System.Reflection;

namespace Shinobytes.Ravenfall.GameServer
{
    class Program
    {
        private static IoC RegisterServices()
        {
            var ioc = new IoC();
            ioc.RegisterCustomShared<IoC>(() => ioc);
            ioc.RegisterShared<IKernel, Kernel>();
            ioc.RegisterShared<ILogger, ConsoleLogger>();
            ioc.RegisterShared<IBinarySerializer, BinarySerializer>();
            ioc.RegisterShared<INetworkPacketTypeRegistry, NetworkPacketTypeRegistry>();
            ioc.RegisterShared<INetworkPacketSerializer, NetworkPacketSerializer>();
            ioc.RegisterShared<IRavenServer, Server>(); // so we can reference this from packet handlers
            ioc.RegisterShared<IServerRegistry, ServerRegistry>();
            ioc.RegisterShared<IMessageBus, MessageBus>();
            ioc.RegisterShared<INetworkPacketController, NetworkPacketController>();

            // providers
            ioc.RegisterShared<IUserProvider, UserProvider>();
            ioc.RegisterShared<IPlayerProvider, PlayerProvider>();
            ioc.RegisterShared<IObjectProvider, ObjectProvider>();
            ioc.RegisterShared<INpcProvider, NpcProvider>();
            ioc.RegisterShared<IItemProvider, ItemProvider>();
            ioc.RegisterShared<IPlayerStatsProvider, PlayerStatsProvider>();
            ioc.RegisterShared<IPlayerStateProvider, PlayerStateProvider>();
            ioc.RegisterShared<INpcStatsProvider, NpcStatsProvider>();
            ioc.RegisterShared<IPlayerInventoryProvider, PlayerInventoryProvider>();
            ioc.RegisterShared<INpcShopInventoryProvider, NpcShopInventoryProvider>();
            ioc.RegisterShared<INpcStateProvider, NpcStateProvider>();

            // processors
            ioc.RegisterShared<IWorldProcessor, WorldProcessor>();

            // services
            ioc.RegisterShared<IAuthService, AuthService>();

            // repositories
            ioc.RegisterShared<IGameObjectRepository, JsonBasedGameObjectRepository>();
            ioc.RegisterShared<IItemRepository, JsonBasedItemRepository>();
            ioc.RegisterShared<INpcRepository, JsonBasedNpcRepository>();
            ioc.RegisterShared<IPlayerRepository, JsonBasedPlayerRepository>();
            ioc.RegisterShared<IEntityActionsRepository, JsonBasedEntityActionsRepository>();


            var logger = ioc.Resolve<ILogger>();
            var packetController = ioc.Resolve<INetworkPacketController>();
            ioc.RegisterCustomShared<IRavenConnectionProvider>(() =>
                new PlayerConnectionProvider(logger, RegisterPacketHandlers(packetController)));

            return ioc;
        }

        private static INetworkPacketController RegisterPacketHandlers(INetworkPacketController controller)
        {
            var packetHandlers = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => !x.IsAbstract && typeof(INetworkPacketHandler).IsAssignableFrom(x));

            foreach (var handler in packetHandlers)
            {
                var packetType = handler.BaseType.GetGenericArguments().FirstOrDefault();
                var packetId = (short)packetType.GetField("OpCode").GetValue(null);
                controller.Register(packetType, handler, packetId);
            }

            return controller;
        }

        static void Main(string[] args)
        {
            Console.Title = "Ravenfall GameServer";

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
