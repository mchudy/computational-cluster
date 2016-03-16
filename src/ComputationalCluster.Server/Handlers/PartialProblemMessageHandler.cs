using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System.Net.Sockets;
using ComputationalCluster.Common.Networking;

namespace ComputationalCluster.Server.Handlers
{
    public class PartialProblemMessageHandler : IMessageHandler<PartialProblemsMessage>
    {
        public void HandleMessage(PartialProblemsMessage message, ITcpConnection connection)
        {
        }
    }
}
