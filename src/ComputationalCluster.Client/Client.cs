using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;
using System.Diagnostics;
using System.IO;


namespace ComputationalCluster.Client
{
    public class Client
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Client));

        private readonly IMessenger messenger;
        private readonly ClientContext context;

        private byte[] problemData;

        public Client(IMessenger messenger, ClientContext context)
        {
            this.messenger = messenger;
            this.context = context;
        }

        public void Start(string problemFilePath)
        {
            if (problemFilePath == null || !File.Exists(problemFilePath))
            {
                throw new ArgumentException("No problem file provided");
            }
            problemData = File.ReadAllBytes(problemFilePath);
            SendSolveRequest(Path.GetFileName(problemFilePath));
        }

        private void SendSolveRequest(string filename)
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
                context.Stopwatch = Stopwatch.StartNew();
                context.ProblemFileName = filename;
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }
    }
}
