using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Serialization;
using System.Collections.Generic;

namespace ComputationalCluster.Common.Messaging
{
    //TODO: timeouts
    public class Messenger : IMessenger
    {
        private readonly IMessageSerializer serializer;
        private readonly IConfiguration configuration;
        private readonly ITcpClientFactory clientFactory;

        public Messenger(IMessageSerializer serializer, IConfiguration configuration,
            ITcpClientFactory clientFactory)
        {
            this.serializer = serializer;
            this.configuration = configuration;
            this.clientFactory = clientFactory;
        }

        public IList<Message> SendMessage(Message message)
        {
            IList<Message> response;
            using (ITcpClient client = clientFactory.Create())
            {
                using (var networkStream = OpenStream(client))
                {
                    var writer = new MessageStreamWriter(networkStream, serializer);
                    writer.WriteMessage(message);
                    var reader = new MessageStreamReader(networkStream, serializer);
                    response = reader.ReadToEnd();
                }
            }
            return response;
        }

        private INetworkStream OpenStream(ITcpClient client)
        {
            client.Connect(configuration.ServerAddress, configuration.ServerPort);
            return client.GetStream();
        }
    }
}
