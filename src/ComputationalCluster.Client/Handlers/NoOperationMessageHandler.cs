using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;

namespace ComputationalCluster.Client.Handlers
{
    public class NoOperationMessageHandler : IResponseHandler<NoOperationMessage>
    {
        private readonly ClientContext context;

        public NoOperationMessageHandler(ClientContext context)
        {
            this.context = context;
        }

        public void HandleResponse(NoOperationMessage message)
        {
            context.BackupServers = message.BackupCommunicationServers;
        }
    }
}
