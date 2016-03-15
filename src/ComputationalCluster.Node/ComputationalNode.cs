using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System;

namespace ComputationalCluster.Node
{
    public class ComputationalNode
    {
        private readonly IMessenger messenger;

        public ComputationalNode(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        public void Start()
        {
            var message = new RegisterMessage
            {
                Type = RegisterType.ComputationalNode,
                ParallelThreads = 3,
                SolvableProblems = new[] { "DVRP" }
            };
            try
            {
                messenger.SendMessage(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
