using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using System;
using System.IO;
using System.Net.Sockets;

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
            var serializer = new MessageSerializer();
            TcpClient client = new TcpClient();
            try
            {
                client.Connect("127.0.0.1", 9000);
                Console.WriteLine("Connected to the server");
                using (var networkStream = client.GetStream())
                using (var reader = new StreamReader(networkStream))
                using (var writer = new StreamWriter(networkStream))
                {
                    var message = new ErrorMessage
                    {
                        ErrorType = ErrorErrorType.ExceptionOccured
                    };
                    string messageString = serializer.Serialize(message);
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
