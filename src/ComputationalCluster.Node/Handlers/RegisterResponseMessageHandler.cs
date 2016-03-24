using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Node.Handlers
{
    public class RegisterResponseMessageHandler : IResponseHandler<RegisterResponseMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RegisterResponseMessageHandler));
        private readonly NodeContext context;
        private readonly IMessenger messenger;
        private readonly IConfiguration configuration;

        public RegisterResponseMessageHandler(NodeContext context, IMessenger messenger, IConfiguration configuration)
        {
            this.context = context;
            this.messenger = messenger;
            this.configuration = configuration;
        }

        public void HandleResponse(RegisterResponseMessage message)
        {
            context.Timeout = message.Timeout;
            context.Id = (int)message.Id;
            logger.Info($"Registered with id {context.Id}");
            Task.Run(() => SendStatus());
        }

        //TODO: separate class
        private void SendStatus()
        {
            while (true)
            {
                try
                {
                    var statusMessage = context.GetStatus();
                    messenger.SendMessage(statusMessage);
                    logger.Debug("Sending status");
                    Thread.Sleep((int)(context.Timeout * 1000 / 2));
                }
                catch (SocketException)
                {
                    logger.Warn("Server failure");
                    if (!RegisterToBackup())
                    {
                        break;
                    }
                }
            }
        }

        private bool RegisterToBackup()
        {
            if (context.BackupServers.Count == 0)
            {
                logger.Error("No backup servers");
                return false;
            }
            logger.Warn("Switching to backup");
            var backupserver = context.BackupServers[0];
            configuration.ServerAddress = backupserver.Address;
            configuration.ServerPort = backupserver.Port;
            return true;
        }
    }
}
