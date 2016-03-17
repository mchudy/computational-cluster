using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Objects;
using log4net;
using System.IO;
using System.Linq;

namespace ComputationalCluster.Server.Handlers
{
    public class StatusMessageHandler : IMessageHandler<StatusMessage>
    {
        private readonly ServerContext context;
        private readonly IServerMessenger messenger;

        public StatusMessageHandler(ServerContext context, IServerMessenger messenger)
        {
            this.context = context;
            this.messenger = messenger;
        }

        public ILog Logger { get; set; }

        public void HandleMessage(StatusMessage message, ITcpConnection connection)
        {
            System.Console.WriteLine("Received status message from component of id: " +message.Id);
            var node = context.Nodes.FirstOrDefault(n => n.Id == (int)message.Id);
            if (node != null)
            {
                HandleNode(node, message, connection.GetStream());
                return;
            }
            var taskManager = context.TaskManagers.FirstOrDefault(t => t.Id == (int)message.Id);
            if (taskManager != null)
            {
                HandleTaskManager(taskManager, message, connection.GetStream());
                return;
            }
            Logger.Error("Status message from not registered component");
        }

        private void HandleTaskManager(TaskManager taskManager, StatusMessage message, Stream stream)
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
                //TODO: move to messenger?
                messenger.SendMessage(new NoOperationMessage
                {
                    BackupCommunicationServers = context.BackupServers.Select(
                        s => new BackupCommunicationServer(s.Address, s.Port)).ToList()
                }, stream);
            }
        }

        private void HandleNode(ComputationalNode node, StatusMessage message, Stream stream)
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
