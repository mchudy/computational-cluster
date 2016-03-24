using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.TaskManager.Handlers
{
    public class RegisterResponseMessageHandler : IResponseHandler<RegisterResponseMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RegisterResponseMessageHandler));

        private readonly TaskManagerContext context;
        private readonly IMessenger messenger;
        private readonly IConfiguration configuration;

        public RegisterResponseMessageHandler(TaskManagerContext context, IMessenger messenger, IConfiguration configuration)
        {
            this.context = context;
            this.messenger = messenger;
            this.configuration = configuration;
        }

        public void HandleResponse(RegisterResponseMessage message)
        {
            context.Timeout = (int)message.Timeout;
            context.Id = (int)message.Id;
            logger.Info($"Registered with id {context.Id}");
            Task.Run(() => SendStatus());
        }

        private void SendStatus()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(context.Timeout * 1000 / 2);
                    logger.Debug("Sending status");
                    var statusMessage = context.GetStatus();
                    messenger.SendMessage(statusMessage);
                }
                catch (SocketException)
                {
                    logger.Error("Server failure");
                    if (!RegisterToBackup())
                        break;
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
            var backupserver = context.BackupServers[0];
            configuration.ServerAddress = backupserver.Address;
            configuration.ServerPort = backupserver.Port;
            return true;
        }
    }
}
