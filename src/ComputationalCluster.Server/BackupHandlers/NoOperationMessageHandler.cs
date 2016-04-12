using ComputationalCluster.Common.Helpers;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System.Linq;

namespace ComputationalCluster.Server.BackupHandlers
{
    public class NoOperationMessageHandler : IResponseHandler<NoOperationMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(NoOperationMessageHandler));

        private readonly IServerContext context;

        public NoOperationMessageHandler(IServerContext context)
        {
            this.context = context;
        }

        public void HandleResponse(NoOperationMessage message)
        {
            if (context.BackupServers.Count == message.BackupCommunicationServers.Count)
            {
                for (int i = 0; i < message.BackupCommunicationServers.Count; i++)
                {
                    var backup = context.BackupServers[i];
                    backup.Address = message.BackupCommunicationServers[i].Address;
                    backup.Port = message.BackupCommunicationServers[i].Port;
                }
            }
            if (!context.IsMasterServerSet)
            {
                context.BackupServers = message.BackupCommunicationServers.Select(s => new BackupServer
                {
                    Address = s.Address,
                    Port = s.Port
                }).ToList();
                SetMasterServer();
                context.BackupServers.Clear();
            }
        }

        private void SetMasterServer()
        {
            // local server is the only backup server
            if (context.BackupServers.Count == 1)
            {
                context.IsMasterServerSet = true;
                return;
            }
            var myIndex = context.BackupServers.FindIndex(b => IPHelper.AreEqual(b.Address, context.LocalAddress) &&
                b.Port == context.Configuration.ListeningPort);
            if (myIndex < 0)
            {
                logger.Error("Local server not registered as backup");
                return;
            }
            context.Configuration.ServerAddress = context.BackupServers[myIndex - 1].Address;
            context.Configuration.ServerPort = context.BackupServers[myIndex - 1].Port;
            logger.Fatal($"Switched master server to {context.Configuration.ServerAddress}:{context.Configuration.ServerPort}");
            context.IsMasterServerSet = true;
        }
    }
}
