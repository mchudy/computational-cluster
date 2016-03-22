using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Objects;
using log4net;
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
            var node = context.Nodes.FirstOrDefault(n => n.Id == (int)message.Id);
            if (node != null)
            {
                HandleNode(node, message, client.GetStream());
                return;
            }
            var taskManager = context.TaskManagers.FirstOrDefault(t => t.Id == (int)message.Id);
            if (taskManager != null)
            {
                HandleTaskManager(taskManager, message, client.GetStream());
                return;
            }
            logger.Error("Status message from not registered component");
        }

        private void HandleTaskManager(TaskManager taskManager, StatusMessage message, INetworkStream stream)
        {
            taskManager.ReceivedStatus = true;
            if (message.Threads.Any(t => t.State == StatusThreadState.Idle))
            {
                var problemToDivide = context.Problems.FirstOrDefault(p => p.Status == ProblemStatus.New &&
                                                            taskManager.SolvableProblems.Contains(p.ProblemType));
                if (problemToDivide != null)
                {
                    problemToDivide.Status = ProblemStatus.Dividing;
                    messenger.SendMessage(new DivideProblemMessage
                    {
                        Id = (ulong)problemToDivide.Id,
                        ComputationalNodes = 10,
                        NodeID = (ulong)taskManager.Id,
                        ProblemType = problemToDivide.ProblemType
                    }, stream);
                }
            }
            else
            {
                //TODO: should be send whenever new backup has been registered/deregistered
                messenger.SendMessage(new NoOperationMessage
                {
                    BackupCommunicationServers = context.BackupServers.Select(
                        s => new BackupCommunicationServer(s.Address, s.Port)).ToList()
                }, stream);
            }
        }

        private void HandleNode(ComputationalNode node, StatusMessage message, INetworkStream stream)
        {
            node.ReceivedStatus = true;
            var problemToSolve = context.Problems.FirstOrDefault(p => p.Status == ProblemStatus.Divided);
            if (problemToSolve != null)
            {
                messenger.SendMessage(new PartialProblemsMessage
                {
                    Id = (ulong)problemToSolve.Id,
                    PartialProblems = problemToSolve.PartialProblems
                }, stream);
                problemToSolve.Status = ProblemStatus.ComputationOngoing;
            }
        }
    }
}
