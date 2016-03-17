using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Serialization;
using log4net;
using System.Collections.Generic;
using System.IO;

namespace ComputationalCluster.Common.Messaging
{
    //TODO: timeouts
    public class Messenger : IMessenger
    {
        private readonly IMessageSerializer serializer;
        private readonly IConfiguration configuration;
        private readonly ITcpConnectionFactory connectionFactory;

        public Messenger(IMessageSerializer serializer, IConfiguration configuration,
            ITcpConnectionFactory connectionFactory)
        {
            this.serializer = serializer;
            this.configuration = configuration;
            this.connectionFactory = connectionFactory;
        }
        public ILog Logger { get; set; }

        public void SendMessageAndClose(Message message)
        {
            using (ITcpConnection client = connectionFactory.Create())
            {
                using (var networkStream = OpenConnection(client))
                using (var writer = new StreamWriter(networkStream))
                {
                    WriteMessage(message, writer);
                }
            }
            //Console.WriteLine("Closed connection to the server");
        }

        public IList<Message> SendMessage(Message message)
        {
            IList<Message> response;
            using (ITcpConnection client = connectionFactory.Create())
            {
                using (var networkStream = OpenConnection(client))
                using (var reader = new StreamReader(networkStream))
                using (var writer = new StreamWriter(networkStream))
                {
                    WriteMessage(message, writer);
                    response = ReadMessages(reader);
                }
            }
            //Console.WriteLine("Closed connection to the server");
            return response;
        }

        private IList<Message> ReadMessages(StreamReader reader)
        {
            string responseString = reader.ReadToEnd();
            string[] messages = responseString.Split(Constants.ETB);
            var response = new List<Message>();
            foreach (var messageXml in messages)
            {
                if (string.IsNullOrEmpty(messageXml))
                {
                    continue;
                }
                Logger.Debug(messageXml);
                response.Add(serializer.Deserialize(messageXml));
            }
            return response;
        }

        private void WriteMessage(Message message, StreamWriter writer)
        {
            string messageString = serializer.Serialize(message);
            writer.Write(messageString);
            writer.Write(Constants.ETB);
            writer.Flush();
        }

        private Stream OpenConnection(ITcpConnection client)
        {
            client.Connect(configuration.ServerAddress, configuration.ServerPort);
            //Console.WriteLine("Connected to the server");
            return client.GetStream();
        }
    }
}
