using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace ComputationalCluster.Common
{
    //TODO: timeouts
    public class Messenger : IMessenger
    {
        public const char ETB = (char)23;

        private readonly IMessageSerializer serializer;

        public Messenger(IMessageSerializer serializer)
        {
            this.serializer = serializer;
        }

        public void SendMessageAndClose(Message message)
        {
            using (TcpClient client = new TcpClient())
            {
                using (var networkStream = OpenConnection(client))
                using (var writer = new StreamWriter(networkStream))
                {
                    WriteMessage(message, writer);
                }
            }
            Console.WriteLine("Closed connection to the server");
        }

        public IList<Message> SendMessage(Message message)
        {
            IList<Message> response;
            using (TcpClient client = new TcpClient())
            {
                using (var networkStream = OpenConnection(client))
                using (var reader = new StreamReader(networkStream))
                using (var writer = new StreamWriter(networkStream))
                {
                    WriteMessage(message, writer);
                    response = ReadMessages(reader);
                }
            }
            Console.WriteLine("Closed connection to the server");
            return response;
        }

        private IList<Message> ReadMessages(StreamReader reader)
        {
            string responseString = reader.ReadToEnd();
            string[] messages = responseString.Split(Constants.ETB);
            var response = new List<Message>();
            foreach (var messageXml in messages)
            {
                Console.WriteLine(messageXml);
                response.Add(serializer.Deserialize(messageXml));
            }
            return response;
        }

        private void WriteMessage(Message message, StreamWriter writer)
        {
            string messageString = serializer.Serialize(message);
            //TODO
            writer.Write(messageString);
            writer.Write(Constants.ETB);
            writer.Flush();
        }

        private NetworkStream OpenConnection(TcpClient client)
        {
            client.Connect("127.0.0.1", 9000);
            Console.WriteLine("Connected to the server");
            return client.GetStream();
        }
    }
}
