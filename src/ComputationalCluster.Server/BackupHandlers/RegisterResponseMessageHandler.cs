using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;
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
                catch (Exception e)
                {
                    logger.Error(e);
                    break;
                }
            }
        }
    }
}
