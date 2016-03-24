using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System.Linq;

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
            context.BackupServers = message.BackupCommunicationServers.Select(s => new BackupServer
            {
                Port = s.Port,
                Address = s.Address
            }).ToList();
        }
    }
}
