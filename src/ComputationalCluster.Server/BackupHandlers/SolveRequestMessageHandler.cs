using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;

namespace ComputationalCluster.Server.BackupHandlers
{
    public class SolveRequestMessageHandler : IResponseHandler<SolveRequestMessage>
    {
        private readonly IServerContext context;

        public SolveRequestMessageHandler(IServerContext context)
        {
            this.context = context;
        }

        public void HandleResponse(SolveRequestMessage message)
        {
            context.BackupMessages.Enqueue(message);
            int id = context.GetNextProblemId();
            var problem = new ProblemInstance
            {
                Id = id,
                Data = message.Data,
                ProblemType = message.ProblemType,
                SolvingTimeout = message.SolvingTimeout,
                Status = ProblemStatus.New
            };
            context.Problems.Add(problem);
        }
    }
}
