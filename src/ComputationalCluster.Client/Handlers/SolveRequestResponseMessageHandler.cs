﻿using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Client.Handlers
{
    public class SolveRequestResponseMessageHandler : IResponseHandler<SolveRequestResponseMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SolveRequestResponseMessageHandler));
        private readonly ClientContext context;
        private readonly IMessenger messenger;
        private readonly IConfiguration configuration;

        public SolveRequestResponseMessageHandler(ClientContext context, IMessenger messenger, IConfiguration configuration)
        {
            this.context = context;
            this.messenger = messenger;
            this.configuration = configuration;
        }

        public void HandleResponse(SolveRequestResponseMessage message)
        {
            context.CurrentProblemId = (int?)message.Id;
            logger.Info($"SolveRequestResponse with id {context.CurrentProblemId}");
            Task.Run(() => WaitForSolution());
        }

        private void WaitForSolution()
        {
            while (context.CurrentProblemId != null)
            {
                Thread.Sleep((int)(context.WaitTime * 1000));
                if (context.CurrentProblemId != null)
                {
                    var message = new SolutionRequestMessage()
                    {
                        Id = (ulong)context.CurrentProblemId
                    };
                    try
                    {
                        messenger.SendMessage(message);
                        logger.Debug("Checking for solution...");
                    }
                    catch (SocketException e)
                    {

                        logger.Warn("Server failure");
                        if (!RegisterToBackup())
                            break;
                    }
                }
            }
        }

        private bool RegisterToBackup()
        {
            if (context.BackupServers.Count == 0)
            {
                logger.Error("No backup servers");
                return false;
            }
            logger.Warn("Switching to backup server");
            var backupserver = context.BackupServers[0];

            configuration.ServerAddress = backupserver.Address;
            configuration.ServerPort = backupserver.Port;
            return true;
        }
    }
}
