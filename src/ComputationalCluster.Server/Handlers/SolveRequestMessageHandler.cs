using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using System.Net.Sockets;

namespace ComputationalCluster.Server.Handlers
{
    public class SolveRequestMessageHandler : IMessageHandler<SolveRequestMessage>
    {
        private readonly IServerMessenger messenger;
        private readonly ServerContext context;

        public SolveRequestMessageHandler(IServerMessenger messenger, ServerContext context)
        {
            this.messenger = messenger;
            this.context = context;
        }

        public void HandleMessage(SolveRequestMessage message, ITcpConnection connection)
        {
            int id = context.GetNextProblemId();
            context.Problems.Add(new ProblemInstance
            {
                Id = id,
                Data = message.Data,
                ProblemType = message.ProblemType,
                SolvingTimeout = message.SolvingTimeout
            });
            SendResponse(connection.GetStream(), id);
            //TODO: add some problems queue for task managers and nodes
        }

        private void SendResponse(NetworkStream stream, int id)
        {
            var response = new SolveRequestResponseMessage
            {
                Id = (ulong)id
            };
            messenger.SendMessage(response, stream);
        }
    }
}
