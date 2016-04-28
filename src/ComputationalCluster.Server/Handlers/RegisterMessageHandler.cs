using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using log4net;
using System.Collections.Generic;

namespace ComputationalCluster.Server.Handlers
{
    public class RegisterMessageHandler : IMessageHandler<RegisterMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RegisterMessageHandler));

        private readonly IServerMessenger messenger;
        private readonly IServerContext context;
        private readonly IStatusChecker statusChecker;

        public RegisterMessageHandler(IServerMessenger messenger, IServerContext context, IStatusChecker statusChecker)
        {
            this.messenger = messenger;
            this.context = context;
            this.statusChecker = statusChecker;
        }

        public void HandleMessage(RegisterMessage message, ITcpClient client)
        {
            int id;
            if (context.IsPrimary)
            {
                id = context.GetNextComponentId();
                message.Id = (ulong)id;
                message.IdSpecified = true;
            }
            else
            {
                messenger.SendMessage(new ErrMessage { ErrorType = ErrorErrorType.NotAPrimaryServer }, client.GetStream());
                return;
            }
            context.BackupMessages.Enqueue(message);
            logger.Info("Received register message");
            switch (message.Type.Type)
            {
                case ClientComponentType.ComputationalNode:
                    HandleNode(message, id);
                    break;
                case ClientComponentType.TaskManager:
                    HandleTaskManager(message, id);
                    break;
                case ClientComponentType.CommunicationServer:
                    HandleBackupServer(client, id, message.Type.port);
                    break;
            }
            if (context.IsPrimary)
            {
                SendResponse(client, id);
            }
        }

        private void SendResponse(ITcpClient client, int id)
        {
            var responseMessage = new RegisterResponseMessage
            {
                Id = (ulong)id,
                Timeout = context.Configuration.Timeout
            };

            List<Message> messages = new List<Message>
            {
                responseMessage,
                context.GetNoOperationMessage()
            };
            messenger.SendMessages(messages, client.GetStream());
        }

        private void HandleBackupServer(ITcpClient client, int id, int port)
        {
            logger.Info($"New backup server registered - id {id}");
            var backup = new BackupServer()
            {
                Address = client.EndPoint.Address.ToString(),
                Port = (ushort)port,
                Id = id
            };
            context.BackupServers.Add(backup);
            statusChecker.Add(backup);
        }

        private void HandleTaskManager(RegisterMessage message, int id)
        {
            logger.Info($"New task manager registered - id {id}");
            var taskManager = new TaskManager
            {
                Id = id,
                SolvableProblems = message.SolvableProblems,
                ThreadsCount = message.ParallelThreads
            };
            context.TaskManagers.Add(taskManager);
            statusChecker.Add(taskManager);
        }

        private void HandleNode(RegisterMessage message, int id)
        {
            logger.Info($"New node registered - id {id}");
            var node = new ComputationalNode
            {
                Id = id,
                SolvableProblems = message.SolvableProblems,
                ThreadsCount = message.ParallelThreads
            };
            context.Nodes.Add(node);
            statusChecker.Add(node);
        }
    }
}
