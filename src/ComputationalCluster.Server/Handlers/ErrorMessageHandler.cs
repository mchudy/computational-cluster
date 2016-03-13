using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using System;
using System.Net.Sockets;

namespace ComputationalCluster.Server.Handlers
{
    public class ErrorMessageHandler : IMessageHandler<ErrorMessage>
    {
        public void HandleMessage(ErrorMessage message, TcpClient client)
        {
            Console.WriteLine("Received error message");
        }
    }
}
