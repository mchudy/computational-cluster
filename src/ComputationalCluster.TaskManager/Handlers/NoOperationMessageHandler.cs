using ComputationalCluster.Common.Messages;
using log4net;

namespace ComputationalCluster.TaskManager.Handlers
{
    public class NoOperationMessageHandler : IResponseHandler<NoOperationMessage>
    {
        private readonly TaskManagerContext context;
        private static readonly ILog logger = LogManager.GetLogger(typeof(NoOperationMessageHandler));

        public NoOperationMessageHandler(TaskManagerContext context)
        {
            this.context = context;
        }

        public void HandleResponse(NoOperationMessage message)
        {
            logger.Debug("Received NoOperation");
            System.Console.WriteLine("halo");
            context.BackupServers = message.BackupCommunicationServers;
        }
    }
}
