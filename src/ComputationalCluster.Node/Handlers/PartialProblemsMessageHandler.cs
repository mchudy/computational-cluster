using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
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
            CreateNewSolutions();
            //TODO
            Task.Run(() => ComputeSolutions(message.Id));
        }

        private void ComputeSolutions(ulong id)
        {
            Thread.Sleep(2000);
            logger.Info("Sending solutions");
            foreach (var solution in context.CurrentSolutions)
            {
                solution.Type = SolutionType.Partial;
                solution.Data = new byte[5];
            }
            messenger.SendMessage(new SolutionMessage
            {
                Id = id,
                Solutions = context.CurrentSolutions.ToArray()
            });
        }

        private void CreateNewSolutions()
        {
            context.CurrentSolutions = new List<Solution>();
            foreach (var partialproblem in context.CurrentPartialProblems)
            {
                context.CurrentSolutions.Add(new Solution
                {
                    TaskId = partialproblem.TaskId,
                    Type = SolutionType.Ongoing
                });
            }
        }
    }
}
