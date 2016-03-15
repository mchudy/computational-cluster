using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System;
using System.Net.Sockets;

namespace ComputationalCluster.Server.Handlers
{
    public class ErrorMessageHandler : IMessageHandler<ErrorMessage>
    {
        public void HandleMessage(ErrorMessage message, NetworkStream stream)
        {
            Console.WriteLine("Received error message");
        }
    }
}
