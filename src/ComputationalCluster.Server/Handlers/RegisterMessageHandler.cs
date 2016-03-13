using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using System;
using System.IO;
using System.Net.Sockets;

namespace ComputationalCluster.Server.Handlers
{
    public class RegisterMessageHandler : IMessageHandler<RegisterMessage>
    {
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
            using (var writer = new StreamWriter(stream))
            {
                var responseMessage = new RegisterResponseMessage()
                {
                    Id = 1,
                    Timeout = 100
                };
                var response = new MessageSerializer().Serialize(responseMessage);
                writer.Write(response);
                writer.Write(Constants.ETB);
                writer.Write(response);
            }
        }
    }
}
