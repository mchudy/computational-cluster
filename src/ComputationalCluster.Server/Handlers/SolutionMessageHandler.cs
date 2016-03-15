using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System.Net.Sockets;

namespace ComputationalCluster.Server.Handlers
{
    public class SolutionMessageHandler : IMessageHandler<SolutionMessage>
    {
        public void HandleMessage(SolutionMessage message, NetworkStream stream)
        {


        }
    }
}
