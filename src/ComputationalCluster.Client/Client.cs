using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
using System;
using System.Collections.Generic;
using System.Threading;


namespace ComputationalCluster.Client
{
    public class Client
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Client));

        private readonly IMessenger messenger;
        private readonly IConfiguration configuration;
        private ulong solutionId;
        private const uint waitTime = 6;
        private byte[] finalSolutionData;

        public Client(IMessenger messenger, IConfiguration configuration)
        {
            this.configuration = configuration;
            this.messenger = messenger;
        }

        public void Start()
        {
            var message = new SolveRequestMessage()
            {
                ProblemType = "DVRP",
                SolvingTimeout = 99999,
                SolvingTimeoutSpecified = true,
                Id = 123,
                IdSpecified = false,
                Data = new byte[] { 1, 2, 3 }
            };
            try
            {
                IList<Message> responses = messenger.SendMessage(message);
                var responseMessage = responses[0] as SolveRequestResponseMessage;
                if (responseMessage != null)
                {
                    var response = responseMessage;

                    solutionId = response.Id;
                    logger.Info($"SolveRequestResponse with id {solutionId}");
                }

            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
            WaitForSolution();
        }

        private void WaitForSolution()
        {
            Thread.Sleep((int)(waitTime * 1000));

            var message = new SolutionRequestMessage()
            {
                Id = solutionId
            };
            try
            {
                IList<Message> responses = messenger.SendMessage(message);
                logger.Debug("Checking for solution...");
                var responseMessage = responses[0] as SolutionMessage;
                if (responseMessage != null)
                {
                    var response = responseMessage;
                    if (response.Solutions == null || response.Solutions.Length == 0)
                    {
                        logger.Warn("Got empty solution list");
                        WaitForSolution();
                    }
                    else if (response.Solutions[0].Type == SolutionType.Ongoing)
                    {
                        logger.Debug("Computations still ongoing");
                        WaitForSolution();
                    }
                    else if (response.Solutions[0].Type == SolutionType.Final)
                    {
                        logger.Info("Final solution received");
                        //finalSolutionData = response.Solutions[0].Data;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }
    }
}
