using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
using System.Linq;

namespace ComputationalCluster.Server.BackupHandlers
{
    public class SolutionMessageHandler : IResponseHandler<SolutionMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SolutionMessageHandler));

        private readonly IServerContext context;

        public SolutionMessageHandler(IServerContext context)
        {
            this.context = context;
        }

        public void HandleResponse(SolutionMessage message)
        {
            context.BackupMessages.Enqueue(message);
            var problem = context.Problems.FirstOrDefault(p => p.Id == (int)message.Id);
            if (problem != null)
            {
                foreach (var solution in message.Solutions)
                {
                    if (solution.TimeoutOccured)
                    {
                        //TODO
                    }
                    switch (solution.Type)
                    {
                        case SolutionType.Partial:
                            logger.Info($"Received {solution.TaskId} partial solution for problem {problem.Id}");
                            var partial = problem.PartialProblems.FirstOrDefault(p => p.Problem.TaskId == solution.TaskId);
                            partial.Solution = solution.Data;
                            partial.State = PartialProblemState.Computed;
                            break;
                        case SolutionType.Final:
                            logger.Info($"Received final solution for problem {problem.Id}");
                            problem.Status = ProblemStatus.Final;
                            problem.FinalSolution = solution.Data;
                            break;
                    }
                }
                if (problem.PartialProblems.All(pp => pp.State == PartialProblemState.Computed) && problem.Status == ProblemStatus.Divided)
                {
                    problem.Status = ProblemStatus.Partial;
                }
            }
        }
    }
}
