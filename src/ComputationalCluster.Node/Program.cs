using ComputationalCluster.Common.Messages;
using System;
using System.IO;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Serialization;

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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(RegisterMessage));
            var message = new RegisterMessage
            {
                Type = RegisterType.ComputationalNode,
                ParallelThreads = 3,
                SolvableProblems = new[] { "DVRP" }
            };
            string messageString;
            using (StringWriter textWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, new XmlWriterSettings { Indent = false }))
                {
                    xmlSerializer.Serialize(xmlWriter, message);
                    messageString = textWriter.ToString();
                }
            }
            return messageString;
        }
    }
}
