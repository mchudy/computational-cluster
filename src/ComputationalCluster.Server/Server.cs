using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Serialization;
using ComputationalCluster.Server.Configuration;
using ComputationalCluster.Server.Extensions;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ComputationalCluster.Server
{
    public class Server
    {
        private readonly IMessageDispatcher messageDispatcher;
        private readonly IMessageSerializer serializer;
        private readonly IServerConfiguration configuration;

        public Server(IMessageDispatcher messageDispatcher, IMessageSerializer serializer,
            IServerConfiguration configuration)
        {
            this.messageDispatcher = messageDispatcher;
            this.serializer = serializer;
            this.configuration = configuration;
        }

        public void Start()
        {
            TcpListener server = new TcpListener(IPAddress.Any, configuration.ListeningPort);
            server.Start();
            Console.WriteLine($"Started listening on {server.LocalEndpoint}");
            while (true)
            {
                AcceptClient(server);
            }
        }

        private void AcceptClient(TcpListener server)
        {
            var client = server.AcceptTcpClient();
            Console.WriteLine($"New connection {client.Client.RemoteEndPoint}");
            try
            {
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream))
                {
                    //TODO: not sure if message must end with ETB... probably should wait some timeout in case it doesn't
                    string messageString = reader.ReadToChar(Constants.ETB);

                    Console.WriteLine($"\nMessage from {client.Client.RemoteEndPoint}\n{messageString}\n");
                    string[] messagesXml = messageString.Split(Constants.ETB);
                    foreach (var xml in messagesXml)
                    {
                        Message message = serializer.Deserialize(xml);
                        messageDispatcher.Dispatch(message, stream);
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Connection lost");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
