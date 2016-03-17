using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Threading;


namespace ComputationalCluster.Client
{
    class Client
    {
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

        public ILog Logger { get; set; }

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

                    Logger.Info($"SolveRequestResponse with id {solutionId}");

                    solutionId = response.Id;
                    Console.WriteLine($"SolveRequestResponse with id {solutionId}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                Console.WriteLine("Checking for solution...");
                var responseMessage = responses[0] as SolutionMessage;
                if (responseMessage != null)
                {
                    var response = responseMessage;
                    if (response.ProblemType == "Ongoing")
                    {
                        Console.WriteLine("Computations still ongoing");

                        WaitForSolution();
                    }
                    else
                    {
                        Console.WriteLine("Final solution recieved");
                        finalSolutionData = response.CommonData;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
