using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Objects;
using log4net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Server.Handlers
{
    public class RegisterMessageHandler : IMessageHandler<RegisterMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(RegisterMessageHandler));

        private readonly IServerMessenger messenger;
        private readonly ServerContext context;

        public RegisterMessageHandler(IServerMessenger messenger, ServerContext context)
        {
            this.messenger = messenger;
            this.context = context;
        }

        public void HandleMessage(RegisterMessage message, ITcpClient client)
        {
            int id = context.GetNextComponentId();
            logger.Info("Received register message");
            if (message.Type == RegisterType.ComputationalNode)
            {
                Console.WriteLine($"New node registered - id {id}");
                var node = new ComputationalNode
                {
                    Id = id,
                    SolvableProblems = message.SolvableProblems,
                    ThreadsCount = message.ParallelThreads
                };
                context.Nodes.Add(node);
                Task.Run(() => CheckNodeTimeout(node));
            }
            else if (message.Type == RegisterType.TaskManager)
            {
                logger.Info($"New task manager registered - id {id}");
                var taskManager = new TaskManager
                {
                    Id = id,
                    SolvableProblems = message.SolvableProblems,
                    ThreadsCount = message.ParallelThreads
                };
                context.TaskManagers.Add(taskManager);
                Task.Run(() => CheckTimeManagerTimeout(taskManager));
            }
            else if (message.Type == RegisterType.CommunicationServer)
            {
                logger.Info($"New backup server registered - id {id}");
                context.BackupServers.Add(new BackupServer
                {
                    Id = id,
                    Address = client.EndPoint.Address.ToString(),
                    Port = (ushort)client.EndPoint.Port
                });
            }
            var responseMessage = new RegisterResponseMessage
            {
                Id = (ulong)id,
                Timeout = context.Configuration.Timeout,
                BackupCommunicationServers = new List<BackupCommunicationServer>()
            };
            messenger.SendMessage(responseMessage, client.GetStream());
        }


        //TODO: class for handling timeouts
        private void CheckNodeTimeout(ComputationalNode node)
        {
            while (true)
            {
                Thread.Sleep((int)(context.Configuration.Timeout * 1000));
                //TODO: ensure atomicity
                if (!node.ReceivedStatus)
                {
                    //TODO: proper deregistration handling
                    context.Nodes.Remove(node);
                    logger.Error($"FAILURE - node with id {node.Id}");
                    break;
                }
                node.ReceivedStatus = false;
            }
        }

        private void CheckTimeManagerTimeout(TaskManager manager)
        {
            while (true)
            {
                Thread.Sleep((int)(context.Configuration.Timeout * 1000));
                if (!manager.ReceivedStatus)
                {
                    context.TaskManagers.Remove(manager);
                    logger.Error($"FAILURE - task manager with id {manager.Id}");
                    break;
                }
                manager.ReceivedStatus = false;
            }
        }
    }
}
