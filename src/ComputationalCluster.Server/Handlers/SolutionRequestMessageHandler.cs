
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Objects;
using log4net;
using System.Collections.Generic;
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
            if (!context.IsPrimary)
            {
                logger.Error("Message not allowed in backup mode");
                messenger.SendMessage(new ErrMessage { ErrorType = ErrorErrorType.NotAPrimaryServer }, client.GetStream());
                return;
            }

            int id = (int)message.Id;
            logger.Debug("Recieved SolutionRequestMessage of id: " + id);
            var problem = context.Problems.First(p => p.Id == id);
            SolutionMessage response = new SolutionMessage();
            if (problem.Status != ProblemStatus.Final)
            {
                response.ProblemType = problem.ProblemType;
                response.Solutions = new[]
                {
                    new Solution
                    {
                        Type = SolutionType.Ongoing
                    }
                };
            }
            else
            {
                response.ProblemType = problem.ProblemType;
                response.Solutions = new[]
                {
                    new Solution
                    {
                        Type = SolutionType.Final,
                        Data = problem.FinalSolution,
                    }
                };
            }
            messenger.SendMessages(new List<Message> { response,
                new NoOperationMessage { BackupCommunicationServers = context.BackupServers }},
                client.GetStream());
        }
    }
}
