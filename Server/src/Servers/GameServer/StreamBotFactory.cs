using RavenfallServer.Packets;
using Shinobytes.Ravenfall.RavenNet.Core;
using Shinobytes.Ravenfall.RavenNet.Models;
using Shinobytes.Ravenfall.RavenNet.Server;
using System.Threading;

namespace RavenfallServer.Providers
{
    public class StreamBotFactory : IStreamBotFactory
    {
        private readonly ILogger logger;

        public StreamBotFactory(ILogger logger)
        {
            this.logger = logger;
        }

        public IStreamBot Create(BotConnection connection)
        {
            return new StreamBot(logger, connection);
        }

        private class StreamBot : IStreamBot
        {
            private readonly ILogger logger;
            private readonly BotConnection connection;
            private int connectionCount = 0;

            public StreamBot(ILogger logger, BotConnection connection)
            {
                this.logger = logger;
                this.connection = connection;
            }

            public int AvailabilityScore => Volatile.Read(ref connectionCount);

            public void Connect(User user)
            {
                Interlocked.Increment(ref connectionCount);
                logger.Debug("StreamBot connecting to stream: " + user.Username);
                connection.Send(new BotStreamConnect
                {
                    StreamID = user.Username
                },
                Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
            }

            public void Disconnect(User user)
            {
                Interlocked.Decrement(ref connectionCount);
                logger.Debug("StreamBot disconnecting from stream: " + user.Username);
                connection.Send(new BotStreamDisconnect
                {
                    StreamID = user.Username
                },
                Shinobytes.Ravenfall.RavenNet.SendOption.Reliable);
            }
        }
    }
}