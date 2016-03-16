using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;

namespace ComputationalCluster.Server.Handlers
{
    public class SolutionRequestMessageHandler : IMessageHandler<SolutionRequestMessage>
    {
        public void HandleMessage(SolutionRequestMessage message, ITcpConnection connection)
        {


        }
    }
}
