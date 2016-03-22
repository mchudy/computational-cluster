
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using log4net;
using System.Linq;

namespace ComputationalCluster.Server.Handlers
{
    public class SolutionRequestMessageHandler : IMessageHandler<SolutionRequestMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SolutionRequestMessageHandler));

        private readonly IServerMessenger messenger;
        private readonly IServerContext context;

        public SolutionRequestMessageHandler(IServerMessenger messenger, IServerContext context)
        {
            this.messenger = messenger;
            this.context = context;
        }

        public void HandleMessage(SolutionRequestMessage message, ITcpClient client)
        {
            int id = (int)message.Id;
            logger.Debug("Recieved SolutionRequestMessage of id: " + id);
            var problem = context.Problems.First(p => p.Id == id);

            SolutionMessage response = new SolutionMessage();

            if (problem.Status != ProblemStatus.Final)
            {
                response.ProblemType = "Ongoing";
            }
            else
            {
                response.CommonData = problem.Data;
                response.ProblemType = "Final";
            }

            SendResponse(client.GetStream(), response);

            //TODO: add some problems queue for task managers and nodes
        }

        private void SendResponse(INetworkStream stream, SolutionMessage response)
        {
            messenger.SendMessage(response, stream);
        }


    }
}
