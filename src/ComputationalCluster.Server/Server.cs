using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

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

                        Console.WriteLine(message.GetType().ToString());
                        //if (message.Type == RegisterType.ComputationalNode)
                        //{
                        //    Console.WriteLine("New node registered");
                        //}
                        //else if (message.Type == RegisterType.TaskManager)
                        //{
                        //    Console.WriteLine("New task manager registered");
                        //}
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
