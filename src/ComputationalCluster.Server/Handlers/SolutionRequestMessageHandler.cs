
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using System.Linq;
using System.IO;

namespace ComputationalCluster.Server.Handlers
{
    public class SolutionRequestMessageHandler : IMessageHandler<SolutionRequestMessage>
    {
        private readonly IServerMessenger messenger;
        private readonly ServerContext context;

        public SolutionRequestMessageHandler(IServerMessenger messenger, ServerContext context)
        {
            this.messenger = messenger;
            this.context = context;
        }

        public void HandleMessage(SolutionRequestMessage message, ITcpConnection connection)
        {
            int id = (int)message.Id;
            System.Console.WriteLine("Recieved SolutionRequestMessage of id: " + id);
            var problem = context.Problems.Where(p => p.Id == id).First();

            SolutionMessage response = new SolutionMessage();

            if(problem.Status != ProblemStatus.Final)
            {
                response.ProblemType = "Ongoing";
            }
            else
            {
                response.CommonData = problem.Data;
                response.ProblemType = "Final";
            }

            SendResponse(connection.GetStream(), response);
            
            //TODO: add some problems queue for task managers and nodes
        }

        private void SendResponse(Stream stream, SolutionMessage response)
        {
            messenger.SendMessage(response, stream);
        }


    }
}
