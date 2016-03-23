using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;

namespace ComputationalCluster.Server.BackupHandlers
{
    public class RegisterResponseMessageHandler : IResponseHandler<RegisterResponseMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RegisterResponseMessageHandler));
        private readonly IServerContext context;

        public RegisterResponseMessageHandler(IServerContext context)
        {
            this.context = context;
        }

        public void HandleResponse(RegisterResponseMessage message)
        {
            logger.Info($"Registered with id {message.Id}");
            context.Id = (int)message.Id;
            context.Configuration.Timeout = message.Timeout;
        }
    }
}
