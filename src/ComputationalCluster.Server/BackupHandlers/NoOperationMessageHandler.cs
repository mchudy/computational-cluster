using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;

namespace ComputationalCluster.Server.BackupHandlers
{
    public class NoOperationMessageHandler : IResponseHandler<NoOperationMessage>
    {
        private readonly IServerContext context;

        public NoOperationMessageHandler(IServerContext context)
        {
            this.context = context;
        }

        public void HandleResponse(NoOperationMessage message)
        {
            context.BackupServers = message.BackupCommunicationServers;
        }
    }
}
