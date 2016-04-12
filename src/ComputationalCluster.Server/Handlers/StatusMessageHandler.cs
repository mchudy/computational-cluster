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
            List<Message> response = new List<Message>();
            if (!context.IsPrimary)
            {
                logger.Error("Message not allowed in backup mode");
                var backup = context.BackupServers.FirstOrDefault(t => t.Id == (int)message.Id);
                if (backup != null)
                {
                    HandleBackupServer(response);
                    messenger.SendMessages(response, client.GetStream());
                }
                else
                {
                    messenger.SendMessage(new ErrMessage { ErrorType = ErrorErrorType.NotAPrimaryServer }, client.GetStream());
                }
                return;
            }

            logger.Debug("Received status message from component of id: " + message.Id);

            var node = context.Nodes.FirstOrDefault(n => n.Id == (int)message.Id);
            if (node != null)
            {
                HandleNode(node, message, response);
                response.Add(context.GetNoOperationMessage());
                messenger.SendMessages(response, client.GetStream());
                return;
            }
            var taskManager = context.TaskManagers.FirstOrDefault(t => t.Id == (int)message.Id);
            if (taskManager != null)
            {
                HandleTaskManager(taskManager, message, response);
                response.Add(context.GetNoOperationMessage());
                messenger.SendMessages(response, client.GetStream());
                return;
            }
            var backupServer = context.BackupServers.FirstOrDefault(t => t.Id == (int)message.Id);
            if (backupServer != null)
            {
                // sychronization queue should be sent only to the first backup server 
                // (backup servers should form a queue)
                if (context.BackupServers[0] == backupServer)
                {
                    HandleBackupServer(response);
                }
                response.Add(context.GetNoOperationMessage());
                messenger.SendMessages(response, client.GetStream());
                return;
            }
            logger.Error("Status message from not registered component");
            messenger.SendMessage(new ErrMessage { ErrorType = ErrorErrorType.UnknownSender }, client.GetStream());
        }

        private void HandleBackupServer(List<Message> response)
        {
            Message backupMessage;
            while (context.BackupMessages.TryDequeue(out backupMessage))
            {
                response.Add(backupMessage);
            }
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
                var divideMessage = new DivideProblemMessage
                {
                    Id = (ulong)problemToDivide.Id,
                    ComputationalNodes = (ulong)context.Nodes.Sum(n => n.ThreadsCount),
                    NodeID = (ulong)taskManager.Id,
                    ProblemType = problemToDivide.ProblemType
                };
                response.Add(divideMessage);
                context.BackupMessages.Enqueue(divideMessage);
                return true;
            }
            return false;
        }

        private void HandleNode(ComputationalNode node, StatusMessage message, IList<Message> messages)
        {
            node.ReceivedStatus = true;
            var idleThreads = message.Threads.Count(t => t.State == StatusThreadState.Idle);
            if (idleThreads == 0) return;
            var problemToSolve = context.Problems.FirstOrDefault(p => p.Status == ProblemStatus.Divided);
            if (problemToSolve != null)
            {
                var partials = problemToSolve.PartialProblems.Where(pp => pp.State == PartialProblemState.New)
                    .Take(idleThreads).ToList();
                if (!partials.Any()) return;
                foreach (var partial in partials)
                {
                    partial.State = PartialProblemState.ComputationOngoing;
                    partial.NodeId = node.Id;
                    partial.Problem.NodeID = (ulong)node.Id;
                }
                messages.Add(new PartialProblemsMessage
                {
                    Id = (ulong)problemToSolve.Id,
                    PartialProblems = partials.Select(p => p.Problem).ToArray()
                });
            }
        }

        private static bool AnyIdleThread(StatusMessage message)
        {
            return message.Threads.Any(t => t.State == StatusThreadState.Idle);
        }
    }
}
