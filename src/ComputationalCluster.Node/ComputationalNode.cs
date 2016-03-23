using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;

namespace ComputationalCluster.Node
{
    public class ComputationalNode
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ComputationalNode));

        private readonly IMessenger messenger;

        public ComputationalNode(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        public void Start()
        {
            Register();
        }

        private void Register()
        {
            var message = new RegisterMessage()
            {
                Type = new ComponentType { Type = ClientComponentType.ComputationalNode },
                SolvableProblems = new[] { "DVRP" },
                ParallelThreads = NodeContext.ParallelThreads
            };
            try
            {
                messenger.SendMessage(message);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }
    }
}
