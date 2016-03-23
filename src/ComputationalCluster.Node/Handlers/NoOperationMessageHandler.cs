using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Node;
using log4net;

namespace ComputationalCluster.TaskManager.Handlers
{
    public class NoOperationMessageHandler : IResponseHandler<NoOperationMessage>
    {
        private readonly NodeContext context;
        private static readonly ILog logger = LogManager.GetLogger(typeof(NoOperationMessageHandler));

        public NoOperationMessageHandler(NodeContext context)
        {
            this.context = context;
        }

        public void HandleResponse(NoOperationMessage message)
        {
            logger.Debug("Received NoOperation");
            context.BackupServers = message.BackupCommunicationServers;
        }
    }
}
