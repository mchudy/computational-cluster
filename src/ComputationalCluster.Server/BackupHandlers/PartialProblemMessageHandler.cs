using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System.Linq;

namespace ComputationalCluster.Server.BackupHandlers
{
    public class PartialProblemMessageHandler : IResponseHandler<PartialProblemsMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PartialProblemMessageHandler));

        private readonly IServerContext context;

        public PartialProblemMessageHandler(IServerContext context)
        {
            this.context = context;
        }

        public void HandleResponse(PartialProblemsMessage message)
        {
            context.BackupMessages.Enqueue(message);
            var problem = context.Problems.FirstOrDefault(p => p.Id == (int)message.Id);
            if (problem != null)
            {
                problem.Status = ProblemStatus.Divided;
                problem.PartialProblems = message.PartialProblems.Select(p => new PartialProblemInstance
                {
                    Problem = p,
                    State = PartialProblemState.New
                }).ToArray();
                logger.Info($"Received partial problems for problem {problem.Id}");
            }
        }
    }
}
