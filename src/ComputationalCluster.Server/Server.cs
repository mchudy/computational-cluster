using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Serialization;
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
        private readonly IServerContext context;
        private readonly IMessenger messenger;
        private readonly ITcpListener listener;

        public Server(IMessageDispatcher messageDispatcher, IMessageSerializer serializer,
            IServerContext context, IMessenger messenger)
        {
            this.messageDispatcher = messageDispatcher;
            this.serializer = serializer;
            this.context = context;
            this.messenger = messenger;
            var tcpListener = new TcpListener(IPAddress.Any, context.Configuration.ListeningPort);
            listener = new TcpListenerAdapter(tcpListener);
        }

        public void Start()
        {
            if (!context.IsPrimary)
            {
                // Register();
            }
            listener.Start();
            logger.Info($"Started listening on {listener.LocalEndpoint}");
            while (true)
            {
                AcceptClient(listener);
            }
        }

        private void Register()
        {
            messenger.SendMessage(new RegisterMessage
            {
                Type = new ComponentType { Type = ClientComponentType.CommunicationServer }
            });
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
