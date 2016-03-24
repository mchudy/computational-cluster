using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Node.Handlers
{
    public class RegisterResponseMessageHandler : IResponseHandler<RegisterResponseMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RegisterResponseMessageHandler));
        private readonly NodeContext context;
        private readonly IMessenger messenger;
        private readonly IConfiguration configuration;

        public RegisterResponseMessageHandler(NodeContext context, IMessenger messenger, IConfiguration configuration)
        {
            this.context = context;
            this.messenger = messenger;
            this.configuration = configuration;
        }

        public void HandleResponse(RegisterResponseMessage message)
        {
            context.Timeout = message.Timeout;
            context.Id = (int)message.Id;
            logger.Info($"Registered with id {context.Id}");
            Task.Run(() => SendStatus());
        }

        //TODO: separate class
        private void SendStatus()
        {
            while (true)
            {
                try
                {
                    var statusMessage = context.GetStatus();
                    messenger.SendMessage(statusMessage);
                    logger.Debug("Sending status");
                    Thread.Sleep((int)(context.Timeout * 1000 / 2));
                }
                catch (SocketException)
                {
                    logger.Error("Server failure");
                    RegisterToBackup();
                    break;
                }
            }
        }

        private void RegisterToBackup()
        {
            if (context.BackupServers.Count == 0)
            {
                logger.Error("No backup servers");
                return;
            }
            var backupserver = context.BackupServers[0];


            var message = new RegisterMessage()
            {
                Type = new ComponentType { Type = ClientComponentType.ComputationalNode },
                SolvableProblems = new[] { "DVRP" },
                ParallelThreads = NodeContext.ParallelThreads
            };

            configuration.ServerAddress = backupserver.Address;
            configuration.ServerPort = backupserver.Port;

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
