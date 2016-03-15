using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System.Linq;
using System.Net.Sockets;

namespace ComputationalCluster.Server.Handlers
{
    public class StatusMessageHandler : IMessageHandler<StatusMessage>
    {
        private readonly ServerContext context;

        public StatusMessageHandler(ServerContext context)
        {
            this.context = context;
        }

        public void HandleMessage(StatusMessage message, NetworkStream stream)
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
                HandleTaskManager(taskManager, message);
                return;
            }
        }

        private void HandleTaskManager(TaskManager taskManager, StatusMessage message)
        {

        }

        private void HandleNode(ComputationalNode node, StatusMessage message)
        {

        }
    }
}
