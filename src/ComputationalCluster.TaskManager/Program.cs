using ComputationalCluster.Common.Messages;
using System;
using System.IO;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Serialization;

namespace ComputationalCluster.TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            StartTaskManager();
            Console.ReadLine();
        }

        private static void StartTaskManager()
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
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(RegisterMessage));
                    var message = new RegisterMessage
                    {
                        Type = RegisterType.TaskManager,
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
                Console.WriteLine("Closed connection to the server");
                client.Close();
            }
        }
    }
}
