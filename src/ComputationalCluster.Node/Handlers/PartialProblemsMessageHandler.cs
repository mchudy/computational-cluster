using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Node.Handlers
{
    public class PartialProblemsMessageHandler : IResponseHandler<PartialProblemsMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PartialProblemsMessageHandler));

        private readonly NodeContext context;
        private readonly IMessenger messenger;

        public PartialProblemsMessageHandler(NodeContext context, IMessenger messenger)
        {
            this.context = context;
            this.messenger = messenger;
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
                Task.Run(() => ComputeSolutions(message.Id, thread, partialProblem));
            }
        }

        private void ComputeSolutions(ulong id, StatusThread thread, PartialProblem partialProblem)
        {
            Thread.Sleep(new Random().Next(7) * 500);
            logger.Info($"Sending solution for partial {partialProblem.TaskId} from problem {id}");
            messenger.SendMessage(new SolutionMessage
            {
                Id = id,
                Solutions = CreateSolution(partialProblem).ToArray()
            });
            logger.Info($"Sending solution for partial {partialProblem.TaskId} from problem {id}");
            context.ReleaseThread(thread);
        }

        private IList<Solution> CreateSolution(PartialProblem partialProblem)
        {
            var solution = new List<Solution>();
            solution.Add(new Solution
            {
                TaskId = partialProblem.TaskId,
                Type = SolutionType.Partial
            });
            return solution;
        }
    }
}
