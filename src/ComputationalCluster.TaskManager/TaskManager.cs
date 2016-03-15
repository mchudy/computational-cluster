using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System;
using System.Collections.Generic;

namespace ComputationalCluster.TaskManager
{
    public class TaskManager
    {
        private const int parallelThreads = 8;

        private readonly IConfiguration configuration;
        private readonly IMessenger messenger;
        private uint timeout;
        private ulong id;

        public TaskManager(IConfiguration configuration, IMessenger messenger)
        {
            this.configuration = configuration;
            this.messenger = messenger;
        }

        public void Start()
        {
            var message = new RegisterMessage()
            {
                SolvableProblems = new[] { "DVRP" },
                ParallelThreads = parallelThreads,
                Type = RegisterType.TaskManager
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
                    Console.WriteLine($"Registered with id {id}");
                }

                var statusMessage = new StatusMessage
                {
                    Id = id
                };
                messenger.SendMessage(statusMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
