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

        public RegisterResponseMessageHandler(TaskManagerContext context, IMessenger messenger)
        {
            this.context = context;
            this.messenger = messenger;
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
                    //TODO: try register to backup
                    break;
                }
            }
        }
    }
}
