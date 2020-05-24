using Shinobytes.Ravenfall.RavenNet.Packets.Client;
using System;
using System.Collections.Concurrent;

namespace Shinobytes.Ravenfall.RavenNet.Modules
{
    public class ChatMessageHandler : IModule
    {
        private readonly ConcurrentQueue<ChatMessage> messageQueue
            = new ConcurrentQueue<ChatMessage>();

        public string Name => "Chat";

        internal void AddChatMessage(ChatMessage data)
        {
            messageQueue.Enqueue(data);
        }

        internal ChatMessage GetNextMessage()
        {
            messageQueue.TryDequeue(out var msg);
            return msg;
        }
    }
}
