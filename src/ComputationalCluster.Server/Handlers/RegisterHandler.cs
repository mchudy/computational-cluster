using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using System;

namespace ComputationalCluster.Server.Handlers
{
    public class RegisterMessageHandler : IMessageHandler<RegisterMessage>
    {
        public void HandleMessage(RegisterMessage message)
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
        }
    }
}
