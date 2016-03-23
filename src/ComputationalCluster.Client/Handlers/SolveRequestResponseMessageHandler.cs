using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Client.Handlers
{
    public class SolveRequestResponseMessageHandler : IResponseHandler<SolveRequestResponseMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SolveRequestResponseMessageHandler));
        private readonly ClientContext context;
        private readonly IMessenger messenger;

        public SolveRequestResponseMessageHandler(ClientContext context, IMessenger messenger)
        {
            this.context = context;
            this.messenger = messenger;
        }

        public void HandleResponse(SolveRequestResponseMessage message)
        {
            context.CurrentProblemId = (int?)message.Id;
            logger.Info($"SolveRequestResponse with id {context.CurrentProblemId}");
            Task.Run(() => WaitForSolution());
        }

        private void WaitForSolution()
        {
            while (context.CurrentProblemId != null)
            {
                Thread.Sleep((int)(context.WaitTime * 1000));
                var message = new SolutionRequestMessage()
                {
                    Id = (ulong)context.CurrentProblemId
                };
                try
                {
                    messenger.SendMessage(message);
                    logger.Debug("Checking for solution...");
                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                }
            }
        }
    }
}
