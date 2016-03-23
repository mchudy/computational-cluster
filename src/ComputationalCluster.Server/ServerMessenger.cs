using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Networking.Factories;
using ComputationalCluster.Common.Serialization;
using System.Collections.Generic;

namespace ComputationalCluster.Server
{
    public class ServerMessenger : IServerMessenger
    {
        private readonly IMessageSerializer serializer;
        private readonly ITcpClientFactory clientFactory;
        private readonly IServerContext context;
        private readonly IMessageStreamFactory streamFactory;

        public ServerMessenger(IMessageSerializer serializer, ITcpClientFactory clientFactory,
            IServerContext context, IMessageStreamFactory streamFactory)
        {
            this.serializer = serializer;
            this.clientFactory = clientFactory;
            this.context = context;
            this.streamFactory = streamFactory;
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

        public void SendToBackup(IList<Message> messages)
        {
            foreach (var backupServer in context.BackupServers)
            {
                using (ITcpClient client = clientFactory.Create())
                {
                    client.Connect(backupServer.Address, backupServer.Port);
                    using (var networkStream = client.GetStream())
                    {
                        var writer = streamFactory.CreateWriter(networkStream);
                        writer.WriteMessages(messages);
                    }
                }
            }
        }
    }
}