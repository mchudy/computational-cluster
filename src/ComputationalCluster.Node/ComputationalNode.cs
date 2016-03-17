using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Node
{
    public class ComputationalNode
    {
        private readonly IMessenger messenger;
        private const int parallelThreads = 2;
        private uint timeout;
        private NodeContext context;
        private ulong id;

        public ComputationalNode(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        public ILog Logger { get; set; }

        public void Start()
        {
            var message = new RegisterMessage()
            {
                Type = RegisterType.ComputationalNode,
                SolvableProblems = new[] { "DVRP" },
                ParallelThreads = parallelThreads
            };
            try
            {
                IList<Message> responses = messenger.SendMessage(message);
                var responseMessage = responses[0] as RegisterResponseMessage;
                if (responseMessage != null)
                {
                    var response = responseMessage;
                    timeout = response.Timeout;
                    id = response.Id;
                    Logger.Info($"Registered with id {id}");
                }
                Task.Run(() => SendStatus());
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }

        private void SendStatus()
        {
            while (true)
            {
                var statusMessage = GetStatus();
                var response = messenger.SendMessage(statusMessage);
                Logger.Debug("Sending status");
                Task.Run(() => HandleResponse(response));
                Thread.Sleep((int)(timeout * 1000 / 2));
            }
        }

        private void HandleResponse(IList<Message> response)
        {
            Message message = response[0];

            if (message is PartialProblemsMessage)
            {
                var partialProblemsMessage = (PartialProblemsMessage)message;
                context.SolvingTimeout = partialProblemsMessage.SolvingTimeout;
                context.CurrentProblemType = partialProblemsMessage.ProblemType;
                context.CurrentPartialProblems = partialProblemsMessage.PartialProblems;
                CreateNewSolutions();
                //TODO
                //Task.Run(() => ComputeSolutions());
            }
        }

        private void ComputeSolutions()
        {
            return;
        }

        private void CreateNewSolutions()
        {
            context.CurrentSolutions = new List<Solution>();
            foreach (var partialproblem in context.CurrentPartialProblems)
            {
                context.CurrentSolutions.Add(new Solution
                {
                    TaskId = partialproblem.TaskId,
                    Type = SolutionType.Ongoing
                });
            }
        }

        private StatusMessage GetStatus()
        {
            var statusMessage = new StatusMessage
            {
                Id = id
            };
            return statusMessage;
        }
    }
}
