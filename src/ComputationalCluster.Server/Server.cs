using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Serialization;
using ComputationalCluster.Server.Configuration;
using log4net;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ComputationalCluster.Server
{
    public class Server
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Server));

        private readonly IMessageDispatcher messageDispatcher;
        private readonly IMessageSerializer serializer;
        private readonly ITcpListener listener;

        public Server(IMessageDispatcher messageDispatcher, IMessageSerializer serializer,
            IServerConfiguration configuration)
        {
            this.messageDispatcher = messageDispatcher;
            this.serializer = serializer;
            var tcpListener = new TcpListener(IPAddress.Any, configuration.ListeningPort);
            listener = new TcpListenerAdapter(tcpListener);
        }

        public void Start()
        {
            listener.Start();
            logger.Info($"Started listening on {listener.LocalEndpoint}");
            while (true)
            {
                AcceptClient(listener);
            }
        }

        private void AcceptClient(ITcpListener server)
        {
            ITcpClient client = server.AcceptTcpClient();
            logger.Debug($"New connection {client.EndPoint}");
            try
            {
                using (var stream = client.GetStream())
                {
                    logger.Debug($"\nMessage from {client.EndPoint}\n");

                    var reader = new MessageStreamReader(stream, serializer);
                    var message = reader.ReadMessage();
                    messageDispatcher.Dispatch(message, client);
                }
            }
            catch (IOException)
            {
                logger.Error("Connection lost");
            }
        }
    }
}
