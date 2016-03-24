using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Networking.Factories;
using log4net;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Common.Messaging
{
    public class Messenger : IMessenger
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Messenger));

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
            bool gotPrimaryServerError = true;
            while (gotPrimaryServerError)
            {
                using (ITcpClient client = clientFactory.Create())
                {
                    using (var networkStream = OpenStream(client))
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
                                logger.Error("Got a NotAPrimaryServer error. Trying again...");
                                Thread.Sleep(500);
                            }
                            else
                            {
                                gotPrimaryServerError = false;
                            }
                        }
                        else
                        {
                            gotPrimaryServerError = false;
                            foreach (var msg in response)
                            {
                                Task.Run(() => dispatcher.Dispatch(msg));
                            }
                        }
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
