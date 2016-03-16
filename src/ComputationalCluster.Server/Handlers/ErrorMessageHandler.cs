using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using System;

namespace ComputationalCluster.Server.Handlers
{
    public class ErrorMessageHandler : IMessageHandler<ErrorMessage>
    {
        public void HandleMessage(ErrorMessage message, ITcpConnection connection)
        {
            Console.WriteLine("Received error message");
        }
    }
}
