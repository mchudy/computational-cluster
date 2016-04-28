using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComputationalCluster.Node.Handlers
{
    public class PartialProblemsMessageHandler : IResponseHandler<PartialProblemsMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PartialProblemsMessageHandler));

        private readonly NodeContext context;
        private readonly IMessenger messenger;
        private readonly ITaskSolverProvider taskSolverProvider;

        public PartialProblemsMessageHandler(NodeContext context, IMessenger messenger,
            ITaskSolverProvider taskSolverProvider)
        {
            this.context = context;
            this.messenger = messenger;
            this.taskSolverProvider = taskSolverProvider;
        }

        public void HandleResponse(PartialProblemsMessage message)
        {
            context.SolvingTimeout = message.SolvingTimeout;
            context.CurrentProblemType = message.ProblemType;
            context.CurrentPartialProblems = message.PartialProblems;
            foreach (var partialProblem in message.PartialProblems)
            {
                logger.Info($"Solving partial {partialProblem.TaskId} for problem {message.Id}");
                var thread = context.TakeThread();
                if (thread == null)
                {
                    logger.Error("No idle thread available");
                }
                Task.Run(() => ComputeSolutions(message, thread, partialProblem));
            }
        }

        private void ComputeSolutions(PartialProblemsMessage msg, StatusThread thread, PartialProblem partialProblem)
        {
            var taskSolver = taskSolverProvider.CreateTaskSolverInstance(msg.ProblemType, null);
            TimeSpan timeout = msg.SolvingTimeout == null ? TimeSpan.MaxValue : TimeSpan.FromSeconds((double)msg.SolvingTimeout);

            var solution = taskSolver.Solve(partialProblem.Data, timeout);

            context.ReleaseThread(thread);
            messenger.SendMessage(new SolutionMessage
            {
                Id = msg.Id,
                Solutions = CreateSolution(partialProblem, solution).ToArray()
            });
            logger.Info($"Sending solution for partial {partialProblem.TaskId} from problem {msg.Id}");
        }

        private static IEnumerable<Solution> CreateSolution(PartialProblem partialProblem, byte[] data)
        {
            var solution = new List<Solution>
            {
                new Solution
                {
                    TaskId = partialProblem.TaskId,
                    Type = SolutionType.Partial,
                    Data = data
                }
            };
            return solution;
        }
    }
}
