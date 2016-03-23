using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Networking.Factories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComputationalCluster.Common.Messaging
{
    //TODO: timeouts
    public class Messenger : IMessenger
    {
        private readonly IConfiguration configuration;
        private readonly ITcpClientFactory clientFactory;
        private readonly IMessageStreamFactory streamFactory;
        private readonly IResponseDispatcher dispatcher;

        public Messenger(IConfiguration configuration, ITcpClientFactory clientFactory,
            IMessageStreamFactory streamFactory, IResponseDispatcher dispatcher)
        {
            this.configuration = configuration;
            this.clientFactory = clientFactory;
            this.streamFactory = streamFactory;
            this.dispatcher = dispatcher;
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
            Task.Run(() =>
            {
                foreach (var msg in response)
                {
                    dispatcher.Dispatch(msg);
                }
            });
            return response;
        }

        private INetworkStream OpenStream(ITcpClient client)
        {
            client.Connect(configuration.ServerAddress, configuration.ServerPort);
            return client.GetStream();
        }
    }
}
