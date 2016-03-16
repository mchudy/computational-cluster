using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using log4net;

namespace ComputationalCluster.Server.Handlers
{
    public class ErrorMessageHandler : IMessageHandler<ErrorMessage>
    {
        public ILog Logger { get; set; }

        public void HandleMessage(ErrorMessage message, ITcpConnection connection)
        {
            Logger.Error("Received error message");
        }
    }
}
