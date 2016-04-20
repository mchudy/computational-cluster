using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;
using System.IO;


namespace ComputationalCluster.Client
{
    public class Client
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Client));

        private readonly IMessenger messenger;

        private byte[] problemData;

        public Client(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        public void Start(string problemFilePath)
        {
            if (problemFilePath == null || !File.Exists(problemFilePath))
            {
                throw new ArgumentException("No problem file provided");
            }
            problemData = File.ReadAllBytes(problemFilePath);
            SendSolveRequest();
        }

        private void SendSolveRequest()
        {
            var message = new SolveRequestMessage()
            {
                ProblemType = "DVRP",
                SolvingTimeout = 99999,
                Data = problemData
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
