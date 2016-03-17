using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using System.Linq;

namespace ComputationalCluster.Server.Handlers
{
    public class SolutionMessageHandler : IMessageHandler<SolutionMessage>
    {
        private readonly ServerContext context;

        public SolutionMessageHandler(ServerContext context)
        {
            this.context = context;
        }

        public void HandleMessage(SolutionMessage message, ITcpConnection connection)
        {
            var problem = context.Problems.FirstOrDefault(p => p.Id == (int)message.Id);
            if (problem != null)
            {
                //problem.Status = ProblemStatus.Partial;
            }
        }
    }
}
