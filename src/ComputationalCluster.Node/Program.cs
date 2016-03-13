using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Serialization;
using System;

namespace ComputationalCluster.Node
{
    class Program
    {
        static void Main(string[] args)
        {
            StartNode();
            Console.ReadLine();
        }

        private static void StartNode()
        {
            var messenger = new Messenger(new MessageSerializer());
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
