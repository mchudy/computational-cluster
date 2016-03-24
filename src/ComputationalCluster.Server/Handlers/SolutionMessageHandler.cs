using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Objects;
using log4net;
using System.Collections.Generic;
using System.Linq;

namespace ComputationalCluster.Server.Handlers
{
    public class SolutionMessageHandler : IMessageHandler<SolutionMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SolutionMessageHandler));

        private readonly IServerContext context;
        private readonly IServerMessenger messenger;

        public SolutionMessageHandler(IServerContext context, IServerMessenger messenger)
        {
            this.context = context;
            this.messenger = messenger;
        }

        public void HandleMessage(SolutionMessage message, ITcpClient client)
        {
            if (context.IsPrimary)
            {
                context.BackupMessages.Enqueue(message);
            }
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
            SendResponse(client);

        }
        private void SendResponse(ITcpClient client)
        {
            if (!context.IsPrimary) return;
            List<Message> messages = new List<Message>();

            messages.Add(context.GetNoOperationMessage());

            messenger.SendMessages(messages, client.GetStream());

        }
    }
}
