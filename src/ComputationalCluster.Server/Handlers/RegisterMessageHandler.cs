using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Serialization;
using ComputationalCluster.Server.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace ComputationalCluster.Server.Handlers
{
    public class RegisterMessageHandler : IMessageHandler<RegisterMessage>
    {
        private readonly IServerConfiguration configuration;
        private readonly IMessageSerializer serializer;

        public RegisterMessageHandler(IServerConfiguration configuration, IMessageSerializer serializer)
        {
            this.configuration = configuration;
            this.serializer = serializer;
        }

        public void HandleMessage(RegisterMessage message, TcpClient client)
        {
            Console.WriteLine("Received register message");
            if (message.Type == RegisterType.ComputationalNode)
            {
                Console.WriteLine("New node registered");
            }
            else if (message.Type == RegisterType.TaskManager)
            {
                Console.WriteLine("New task manager registered");
            }
            using (var stream = client.GetStream())
            {
                var responseMessage = new RegisterResponseMessage
                {
                    Id = 1,
                    Timeout = configuration.Timeout
                };
                SendMessages(new List<Message> { responseMessage, responseMessage }, stream);
            }
        }

        public void SendMessages(IList<Message> messages, NetworkStream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                foreach (var message in messages)
                {
                    string xml = serializer.Serialize(message);
                    writer.Write(xml);
                    writer.Write(Constants.ETB);
                }
                writer.Flush();
            }
        }
    }
}
