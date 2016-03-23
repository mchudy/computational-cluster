using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Objects;
using log4net;
using System.Collections.Generic;
using System.Linq;

namespace ComputationalCluster.Server.Handlers
{
    public class StatusMessageHandler : IMessageHandler<StatusMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(StatusMessageHandler));

        private readonly IServerContext context;
        private readonly IServerMessenger messenger;

        public StatusMessageHandler(IServerContext context, IServerMessenger messenger)
        {
            this.context = context;
            this.messenger = messenger;
        }

        public void HandleMessage(StatusMessage message, ITcpClient client)
        {
            logger.Debug("Received status message from component of id: " + message.Id);

            List<Message> response = new List<Message>
            {
                new NoOperationMessage() {BackupCommunicationServers = context.BackupServers}
            };

            var node = context.Nodes.FirstOrDefault(n => n.Id == (int)message.Id);
            if (node != null)
            {
                HandleNode(node, message, response);
                messenger.SendMessages(response, client.GetStream());
                return;
            }
            var taskManager = context.TaskManagers.FirstOrDefault(t => t.Id == (int)message.Id);
            if (taskManager != null)
            {
                HandleTaskManager(taskManager, message, response);
                messenger.SendMessages(response, client.GetStream());
                return;
            }
            logger.Error("Status message from not registered component");
        }

        private void HandleTaskManager(TaskManager taskManager, StatusMessage message, IList<Message> response)
        {
            taskManager.ReceivedStatus = true;
            if (AnyIdleThread(message))
            {
                if (TryDivideProblem(taskManager, response)) return;
                TryMergeProblem(taskManager, response);
            }
            else
            {
                //TODO: should be send whenever new backup has been registered/deregistered
                response.Add(new NoOperationMessage
                {
                    BackupCommunicationServers = context.BackupServers.Select(
                        s => new BackupCommunicationServer(s.Address, s.Port)).ToList()
                });
            }
        }

        private void TryMergeProblem(TaskManager taskManager, IList<Message> response)
        {
            var problemToMerge = context.Problems.FirstOrDefault(p => p.Status == ProblemStatus.Partial &&
                                                                      taskManager.SolvableProblems.Contains(p.ProblemType));
            if (problemToMerge != null)
            {
                logger.Info($"Sending problem {problemToMerge.Id} for task manager to merge");
                problemToMerge.Status = ProblemStatus.Merging;
                var solutions = new Solution[problemToMerge.PartialProblems.Length];
                for (int i = 0; i < problemToMerge.PartialProblems.Length; i++)
                {
                    solutions[i] = new Solution
                    {
                        Type = SolutionType.Partial,
                        TaskId = (ulong)i,
                    };
                }
                response.Add(new SolutionMessage
                {
                    Id = (ulong)problemToMerge.Id,
                    ProblemType = problemToMerge.ProblemType,
                    Solutions = solutions
                });
            }
        }

        private bool TryDivideProblem(TaskManager taskManager, IList<Message> response)
        {
            var problemToDivide = context.Problems.FirstOrDefault(p => p.Status == ProblemStatus.New &&
                                                                       taskManager.SolvableProblems.Contains(p.ProblemType));
            if (problemToDivide != null)
            {
                logger.Info($"Sending problem {problemToDivide.Id} for task manager to divide");
                problemToDivide.Status = ProblemStatus.Dividing;
                response.Add(new DivideProblemMessage
                {
                    Id = (ulong)problemToDivide.Id,
                    ComputationalNodes = 10 /*TODO*/,
                    NodeID = (ulong)taskManager.Id,
                    ProblemType = problemToDivide.ProblemType
                });
                return true;
            }
            return false;
        }

        private void HandleNode(ComputationalNode node, StatusMessage message, IList<Message> messages)
        {
            if (!AnyIdleThread(message)) return;
            node.ReceivedStatus = true;
            var problemToSolve = context.Problems.FirstOrDefault(p => p.Status == ProblemStatus.Divided);
            if (problemToSolve != null)
            {
                var partial = problemToSolve.PartialProblems.FirstOrDefault(pp => pp.State == PartialProblemState.New);
                if (partial == null) return;
                messages.Add(new PartialProblemsMessage
                {
                    Id = (ulong)problemToSolve.Id,
                    PartialProblems = new[] { partial.Problem }
                });
                partial.State = PartialProblemState.ComputationOngoing;
                partial.NodeId = node.Id;
            }
        }

        private static bool AnyIdleThread(StatusMessage message)
        {
            return message.Threads.Any(t => t.State == StatusThreadState.Idle);
        }
    }
}
