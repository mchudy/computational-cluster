using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using System;
using System.IO;
using System.Net.Sockets;

namespace ComputationalCluster.Node
{
    class Program
    {
        static void Main(string[] args)
        {
            StartNode();
            Console.ReadLine();
        }

        private static void StartNode()
        {
            TcpClient client = new TcpClient();
            try
            {
                client.Connect("127.0.0.1", 9000);
                Console.WriteLine("Connected to the server");

                using (var networkStream = client.GetStream())
                using (var reader = new StreamReader(networkStream))
                using (var writer = new StreamWriter(networkStream))
                {
                    var messageString = GetRegisterMessage();
                    writer.WriteLine(messageString);
                    writer.Flush();
                    networkStream.Flush();
                    Console.WriteLine(reader.ReadLine());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                client.Close();
                Console.WriteLine("Closed connection to the server");
            }
        }

        private static string GetRegisterMessage()
        {
            var message = new RegisterMessage
            {
                Type = RegisterType.ComputationalNode,
                ParallelThreads = 3,
                SolvableProblems = new[] { "DVRP" }
            };
            return new MessageSerializer().Serialize(message);
        }
    }
}
