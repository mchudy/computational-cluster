using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Networking.Factories;
using System.Collections.Generic;

namespace ComputationalCluster.Common.Messaging
{
    //TODO: timeouts
    public class Messenger : IMessenger
    {
        private readonly IConfiguration configuration;
        private readonly ITcpClientFactory clientFactory;
        private readonly IMessageStreamFactory streamFactory;

        public Messenger(IConfiguration configuration,
            ITcpClientFactory clientFactory, IMessageStreamFactory streamFactory)
        {
            this.configuration = configuration;
            this.clientFactory = clientFactory;
            this.streamFactory = streamFactory;
        }

        public IList<Message> SendMessage(Message message)
        {
            IList<Message> response;
            using (ITcpClient client = clientFactory.Create())
            {
                using (var networkStream = OpenStream(client))
                {
                    var writer = streamFactory.CreateWriter(networkStream);
                    writer.WriteMessage(message);
                    var reader = streamFactory.CreateReader(networkStream);
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
