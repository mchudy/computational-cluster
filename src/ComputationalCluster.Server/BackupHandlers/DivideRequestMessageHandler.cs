using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System.Linq;

namespace ComputationalCluster.Server.BackupHandlers
{
    public class DivideRequestMessageHandler : IResponseHandler<DivideProblemMessage>
    {
        private readonly IServerContext context;

        public DivideRequestMessageHandler(IServerContext context)
        {
            this.context = context;
        }

        public void HandleResponse(DivideProblemMessage message)
        {
            var problem = context.Problems.FirstOrDefault(p => p.Id == (int)message.Id);
            if (problem != null)
            {
                problem.Status = ProblemStatus.Dividing;
            }
        }
    }
}
