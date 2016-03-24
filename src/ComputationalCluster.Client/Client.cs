using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;


namespace ComputationalCluster.Client
{
    public class Client
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Client));

        private readonly IMessenger messenger;

        public Client(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        public void Start()
        {
            var message = new SolveRequestMessage()
            {
                ProblemType = "DVRP",
                SolvingTimeout = 99999,
                Id = 123,
                Data = new byte[] { 1, 2, 3 }
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
