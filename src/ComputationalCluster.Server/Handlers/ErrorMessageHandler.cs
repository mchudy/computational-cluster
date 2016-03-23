using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using log4net;

namespace ComputationalCluster.Server.Handlers
{
    public class ErrorMessageHandler : IMessageHandler<ErrMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ErrorMessageHandler));

        public void HandleMessage(ErrMessage message, ITcpClient client)
        {
            logger.Error("Received error message");
        }
    }
}
