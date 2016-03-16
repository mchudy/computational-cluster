using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Objects;
using System.Linq;
using System.Net.Sockets;

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

        public void HandleMessage(StatusMessage message, ITcpConnection connection)
        {
            var node = context.Nodes.FirstOrDefault(n => n.Id == (int)message.Id);
            if (node != null)
            {
                HandleNode(node, message);
                return;
            }
            var taskManager = context.TaskManagers.FirstOrDefault(t => t.Id == (int)message.Id);
            if (taskManager != null)
            {
                HandleTaskManager(taskManager, message, connection.GetStream());
                return;
            }
        }

        private void HandleTaskManager(TaskManager taskManager, StatusMessage message, NetworkStream stream)
        {

            taskManager.ReceivedStatus = true;
            messenger.SendMessage(new NoOperationMessage
            {
                BackupCommunicationServers = context.BackupServers.Select(
                    s => new BackupCommunicationServer(s.Address, s.Port)).ToList()
            }, stream);
        }

        private void HandleNode(ComputationalNode node, StatusMessage message)
        {

        }
    }
}
