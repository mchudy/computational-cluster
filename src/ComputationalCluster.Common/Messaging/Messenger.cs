using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Networking.Factories;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Common.Messaging
{
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

        public void SendMessage(Message message)
        {
            using (ITcpClient client = clientFactory.Create())
            {
                using (var networkStream = OpenStream(client))
                {
                    while (true)
                    {
                        var writer = streamFactory.CreateWriter(networkStream);
                        writer.WriteMessage(message);
                        var reader = streamFactory.CreateReader(networkStream);
                        var response = reader.ReadToEnd();
                        if (response.Count > 0 && response[0] is ErrMessage)
                        {
                            var err = (ErrMessage)response[0];
                            if (err.ErrorType == ErrorErrorType.NotAPrimaryServer)
                            {
                                Thread.Sleep(100);
                                continue;
                            }
                        }
                        foreach (var msg in response)
                        {
                            Task.Run(() => dispatcher.Dispatch(msg));
                        }
                        break;
                    }

                }
            }
        }

        private INetworkStream OpenStream(ITcpClient client)
        {
            client.Connect(configuration.ServerAddress, configuration.ServerPort);
            return client.GetStream();
        }
    }
}
