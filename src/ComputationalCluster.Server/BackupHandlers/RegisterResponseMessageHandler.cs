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

        public RegisterResponseMessageHandler(IServerContext context, IMessenger messenger)
        {
            this.context = context;
            this.messenger = messenger;
        }

        public void HandleResponse(RegisterResponseMessage message)
        {
            logger.Info($"Registered with id {message.Id}");
            context.Id = (int)message.Id;
            context.Configuration.Timeout = message.Timeout;
            Task.Run(() => SendStatus());
        }

        //TODO: separate class
        private void SendStatus()
        {
            while (true)
            {
                try
                {
                    var statusMessage = new StatusMessage { Id = (ulong)context.Id };
                    messenger.SendMessage(statusMessage);
                    logger.Debug("Sending status");
                    Thread.Sleep((int)(context.Configuration.Timeout * 1000 / 2));
                }
                catch (SocketException)
                {
                    SwitchToPrimary();
                    break;
                }
            }
        }

        private void SwitchToPrimary()
        {
            logger.Error("Primary server failure");
            logger.Info("Switching to primary mode");
            context.Configuration.Mode = ServerMode.Primary;
            context.BackupServers.RemoveAt(0);
        }
    }
}
