using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Serialization;
using System.Collections.Generic;

namespace ComputationalCluster.Server
{
    public class ServerMessenger : IServerMessenger
    {
        private readonly IMessageSerializer serializer;

        public ServerMessenger(IMessageSerializer serializer)
        {
            this.serializer = serializer;
        }

        public void SendMessage(Message message, INetworkStream stream)
        {
            var writer = new MessageStreamWriter(stream, serializer);
            writer.WriteMessage(message);
        }

        public void SendMessages(IList<Message> messages, INetworkStream stream)
        {
            var writer = new MessageStreamWriter(stream, serializer);
            writer.WriteMessages(messages);
        }
    }
}