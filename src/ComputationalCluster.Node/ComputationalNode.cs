using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
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
        private ulong id;

        public ComputationalNode(IMessenger messenger)
        {
            this.messenger = messenger;
        }

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
                    Console.WriteLine($"Registered with id {id}");
                }
                Task.Run(() => SendStatus());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void SendStatus()
        {
            while (true)
            {
                var statusMessage = GetStatus();
                messenger.SendMessage(statusMessage);
                Console.WriteLine("Sending status");
                Thread.Sleep((int)(timeout * 1000));
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
