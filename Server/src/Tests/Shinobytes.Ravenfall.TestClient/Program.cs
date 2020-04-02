using Shinobytes.Ravenfall.RavenNet;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Modules;
using Shinobytes.Ravenfall.RavenNet.Packets;
using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using Shinobytes.Ravenfall.RavenNet.Serializers;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Shinobytes.Ravenfall.TestClient
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
            ioc.RegisterShared<IRavenClient, Client>(); // so we can reference this from packet handlers
            ioc.RegisterShared<INetworkPacketController, NetworkPacketController>();
            ioc.RegisterShared<IMessageBus, MessageBus>();
            ioc.RegisterShared<IModuleManager, ModuleManager>();

            return ioc;
        }

        static void Main(string[] args)
        {
            const int frontServerPort = 8133;

            Console.Title = "Ravenfall - Headerless Client";

            var ioc = RegisterServices();
            var logger = ioc.Resolve<ILogger>();
            using (var client = ioc.Resolve<IRavenClient>())
            {

                // IPAddress.Loopback
                client.Connect(IPAddress.Parse("51.89.117.205"), frontServerPort);
                var auth = client.Modules.GetModule<Authentication>();


                var count = 10_000;
                var sw = new Stopwatch();
                sw.Start();
                for (var i = 0; i < count; ++i)
                {
                    client.SendReliable(new AuthRequest() { ClientVersion = "TEST", Password = "LUL", Username = "User123" });

                    System.Threading.Thread.SpinWait(2);
                }

                sw.Stop();
                logger.WriteLine("@yel@" + count + " packets sent in @whi@" + sw.Elapsed.TotalSeconds + "@yel@ seconds. Avg: @whi@" + (count / sw.Elapsed.TotalSeconds) + " @yel@per second");

                while (true)
                {

                    if (!auth.Authenticated)
                    {
                        if (auth.Authenticating)
                        {
                            System.Threading.Thread.SpinWait(1);
                            continue;
                        }

                        logger.Write("Enter username: ");
                        var username = Console.ReadLine();
                        var password = "";

                        logger.Write("Enter password: ");

                        ConsoleKeyInfo keyInfo;
                        while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Enter)
                        {
                            password += keyInfo.KeyChar;
                        }

                        logger.WriteLine("");
                        logger.Debug("Logging in... Please wait");

                        auth.Authenticate(username, password);
                        continue;
                    }

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
