using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace ComputationalCluster.Server
{
    public class Server
    {
        //TODO: read from config
        private readonly IPAddress address = IPAddress.Parse("127.0.0.1");
        private const int port = 9000;

        //TODO: DI
        private readonly IMessageSerializer serializer = new MessageSerializer();

        public void Start()
        {
            TcpListener server = new TcpListener(address, port);
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
                {
                    using (var reader = new StreamReader(stream))
                    using (var writer = new StreamWriter(stream))
                    {
                        string messageString = reader.ReadLine();
                        Console.WriteLine($"\nMessage from {client.Client.RemoteEndPoint}\n{messageString}\n");
                        Message message = serializer.Deserialize(messageString);
                        HandleMessage(message);
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

        //TODO: DI container?
        private void HandleMessage(Message message)
        {
            Type interfaceType = typeof(IMessageHandler<>).MakeGenericType(message.GetType());
            var type = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .FirstOrDefault(p => interfaceType.IsAssignableFrom(p));
            var instance = Activator.CreateInstance(type);
            type.GetMethod("HandleMessage").Invoke(instance, new object[] { message });
        }
    }
}
