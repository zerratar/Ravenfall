using Shinobytes.Ravenfall.RavenNet.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shinobytes.Ravenfall.RavenNet.Packets
{
    public class NetworkPacketController : INetworkPacketController
    {
        private readonly ConcurrentDictionary<string, Lazy<INetworkPacketHandler>> packetHandlers = new ConcurrentDictionary<string, Lazy<INetworkPacketHandler>>();
        private readonly ConcurrentDictionary<string, MethodInfo> packetHandlerMethod = new ConcurrentDictionary<string, MethodInfo>();
        private readonly List<Filter> filters = new List<Filter>();
        private readonly object filterMutex = new object();

        private readonly IoC ioc;
        private readonly INetworkPacketTypeRegistry packetRegistry;
        private readonly INetworkPacketSerializer packetSerializer;

        public NetworkPacketController(IoC ioc, INetworkPacketTypeRegistry packetRegistry, INetworkPacketSerializer packetSerializer)
        {
            this.ioc = ioc;
            this.packetRegistry = packetRegistry;
            this.packetSerializer = packetSerializer;
        }

        public INetworkPacketController Register<TPacket, TPacketHandler>() where TPacketHandler : INetworkPacketHandler<TPacket>
        {
            // let it throw.
            try
            {
                var packetId = (short)typeof(TPacket).GetField("OpCode").GetValue(null);
                return Register<TPacket, TPacketHandler>(packetId);
            }
            catch
            {
                throw new Exception("Missing: public const short OpCode = <num>; on " + typeof(TPacket).FullName);
            }
        }

        public INetworkPacketController Register<TPacket>()
        {
            // let it throw.
            try
            {
                var packetId = (short)typeof(TPacket).GetField("OpCode").GetValue(null);
                return Register<TPacket>(packetId);
            }
            catch
            {
                throw new Exception("Missing: public const short OpCode = <num>; on " + typeof(TPacket).FullName);
            }
        }

        public INetworkPacketController Register<TPacket, TPacketHandler>(short packetId) where TPacketHandler : INetworkPacketHandler<TPacket>
        {
            var key = typeof(TPacket).Name;
            ioc.Register<INetworkPacketHandler<TPacket>, TPacketHandler>();
            packetHandlers[key] = new Lazy<INetworkPacketHandler>(() => ioc.Resolve<INetworkPacketHandler<TPacket>>(), true);
            packetHandlerMethod[key] = typeof(TPacketHandler).GetMethod("Handle");
            packetRegistry.Register<TPacket>(packetId);
            return this;
        }

        public INetworkPacketController Register(Type packetType, Type packetHandlerType, short packetId)
        {
            var key = packetType.Name;
            var packetHandlerInterface = typeof(INetworkPacketHandler<>).MakeGenericType(packetType);

            ioc.Register(packetHandlerInterface, packetHandlerType);
            packetHandlers[key] = new Lazy<INetworkPacketHandler>(() => (INetworkPacketHandler)ioc.Resolve(packetHandlerInterface), true);
            packetHandlerMethod[key] = packetHandlerType.GetMethod("Handle");
            packetRegistry.Register(packetType, packetId);
            return this;
        }

        public INetworkPacketController Register<TPacket>(short id)
        {
            this.packetRegistry.Register<TPacket>(id);
            return this;
        }

        public void Handle(IRavenNetworkConnection connection, NetworkPacket packet, SendOption sendOption)
        {
            if (packet.Data == null) return;
            var key = packet.Data.GetType().Name;
            if (packetHandlers.TryGetValue(key, out var handler))
            {
                packetHandlerMethod[key].Invoke(handler.Value, new object[] { packet.Data, connection, sendOption });
                return;
            }

            lock (filterMutex)
            {
                var filter = filters.FirstOrDefault(x => x.CanApply(packet.Data.GetType()));
                if (filter != null && filter.Apply(packet.Data))
                {
                    filters.Remove(filter);
                }
            }
        }

        public void HandlePacketData(IRavenNetworkConnection connection, MessageReader message, SendOption sendOption)
        {
            NetworkPacket packet = packetSerializer.Deserialize(message);
            if (packet != null)
            {
                Handle(connection, packet, sendOption);
            }
        }

        public void Send<T>(Connection connection, short packetId, T packet, SendOption sendOption)
        {
            connection.SendBytes(packetSerializer.Serialize(new NetworkPacket()
            {
                Id = packetId,
                Data = packet
            }), sendOption);
        }

        public void Send<T>(Connection connection, T packet, SendOption sendOption)
        {
            if (!packetRegistry.TryGetId(typeof(T), out var id))
            {
                Register<T>();
            }

            if (packetRegistry.TryGetId(typeof(T), out id))
            {
                Send(connection, id, packet, sendOption);
                return;
            }

            throw new Exception("Unable to send packet. Failed to lookup packetid for type " + typeof(T).FullName);
        }

        public void AddFilter<TResponse>(Func<TResponse, bool> filter)
        {
            lock (filterMutex)
            {
                this.filters.Add(new Filter(typeof(TResponse), filter));
            }
        }

        private class Filter
        {
            private readonly Type type;
            private readonly object method;
            private readonly MethodInfo invoke;

            public Filter(Type type, object method)
            {
                this.type = type;
                this.method = method;
                this.invoke = method.GetType().GetMethod("Invoke");
            }
            public bool CanApply(Type type)
            {
                return this.type == type;
            }

            public bool Apply(object item)
            {
                return (bool)invoke.Invoke(method, new object[] { item });
            }
        }
    }
}