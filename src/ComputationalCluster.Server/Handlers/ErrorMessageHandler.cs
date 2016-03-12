using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using System;

namespace ComputationalCluster.Server.Handlers
{
    public class ErrorMessageHandler : IMessageHandler<ErrorMessage>
    {
        public void HandleMessage(ErrorMessage message)
        {
            Console.WriteLine("Received error message");
        }
    }
}
