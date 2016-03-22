using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using System.Linq;

namespace ComputationalCluster.Server.Handlers
{
    public class SolutionMessageHandler : IMessageHandler<SolutionMessage>
    {
        private readonly IServerContext context;

        public SolutionMessageHandler(IServerContext context)
        {
            this.context = context;
        }

        public void HandleMessage(SolutionMessage message, ITcpClient client)
        {
            var problem = context.Problems.FirstOrDefault(p => p.Id == (int)message.Id);
            if (problem != null)
            {
                //problem.Status = ProblemStatus.Partial;
            }
        }
    }
}
