using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Server.Configuration;
using log4net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Server.BackupHandlers
{
    public class RegisterResponseMessageHandler : IResponseHandler<RegisterResponseMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RegisterResponseMessageHandler));
        private readonly IServerContext context;
        private readonly IMessenger messenger;
        private readonly IStatusChecker checker;

        public RegisterResponseMessageHandler(IServerContext context, IMessenger messenger, IStatusChecker checker)
        {
            this.context = context;
            this.messenger = messenger;
            this.checker = checker;
        }

        public void HandleResponse(RegisterResponseMessage message)
        {
            logger.Info($"Registered with id {message.Id}");
            context.Id = (int)message.Id;
            context.Configuration.Timeout = message.Timeout;
            Task.Run(() => SendStatus());
        }

        private void SendStatus()
        {
            while (!context.IsPrimary)
            {
                try
                {
                    var statusMessage = new StatusMessage { Id = (ulong)context.Id };
                    messenger.SendMessage(statusMessage);
                    logger.Debug("Sending status");
                    Thread.Sleep((int)(context.Configuration.Timeout * 1000 / 2));
                }
                //TODO: custom exception
                catch (SocketException)
                {
                    SwitchToPrimary();
                    break;
                }
            }
        }

        private void SwitchToPrimary()
        {
            logger.Warn("Primary server failure");
            logger.Info("Switching to primary mode");
            context.BackupServers.RemoveAt(0);
            context.Configuration.Mode = ServerMode.Primary;
            foreach (var node in context.Nodes)
            {
                node.ReceivedStatus = true;
                checker.Add(node);
            }
            foreach (var taskManager in context.TaskManagers)
            {
                taskManager.ReceivedStatus = true;
                checker.Add(taskManager);
            }
        }
    }
}
