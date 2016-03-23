using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Node.Handlers
{
    public class RegisterResponseMessageHandler : IResponseHandler<RegisterResponseMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RegisterResponseMessageHandler));
        private readonly NodeContext context;
        private readonly IMessenger messenger;

        public RegisterResponseMessageHandler(NodeContext context, IMessenger messenger)
        {
            this.context = context;
            this.messenger = messenger;
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
                catch (Exception e)
                {
                    logger.Error(e);
                    break;
                }
            }
        }
    }
}
