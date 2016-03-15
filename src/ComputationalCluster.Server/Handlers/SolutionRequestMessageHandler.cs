using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System.Net.Sockets;

namespace ComputationalCluster.Server.Handlers
{
    public class SolutionRequestMessageHandler : IMessageHandler<SolutionRequestMessage>
    {
        public void HandleMessage(SolutionRequestMessage message, NetworkStream stream)
        {


        }
    }
}
