using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Networking;
using ComputationalCluster.Common.Objects;
using ComputationalCluster.Server.Configuration;
using log4net;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.Server.Handlers
{
    public class RegisterMessageHandler : IMessageHandler<RegisterMessage>
    {
        private readonly IServerConfiguration configuration;
        private readonly IServerMessenger messenger;
        private readonly ServerContext context;

        public RegisterMessageHandler(IServerConfiguration configuration, IServerMessenger messenger,
            ServerContext context)
        {
            this.configuration = configuration;
            this.messenger = messenger;
            this.context = context;
        }

        public ILog Logger { get; set; }

        public void HandleMessage(RegisterMessage message, ITcpConnection connection)
        {
            int id = context.GetNextComponentId();
            Logger.Info("Received register message");
            if (message.Type == RegisterType.ComputationalNode)
            {
                Console.WriteLine($"New node registered - id {id}");
                context.Nodes.Add(new ComputationalNode
                {
                    Id = id,
                    SolvableProblems = message.SolvableProblems,
                    ThreadsCount = message.ParallelThreads
                });
            }
            else if (message.Type == RegisterType.TaskManager)
            {
                Logger.Info($"New task manager registered - id {id}");
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
                Logger.Info($"New backup server registered - id {id}");
                context.BackupServers.Add(new BackupServer
                {
                    Id = id,
                    Address = connection.EndPoint.Address.ToString(),
                    Port = (ushort)connection.EndPoint.Port
                });
            }
            var responseMessage = new RegisterResponseMessage
            {
                Id = (ulong)id,
                Timeout = configuration.Timeout,
                BackupCommunicationServers = new List<BackupCommunicationServer>()
            };
            messenger.SendMessage(responseMessage, connection.GetStream());
        }

        private void CheckTimeManagerTimeout(TaskManager manager)
        {
            while (true)
            {
                Thread.Sleep((int)(configuration.Timeout * 1000));
                //TODO: ensure atomicity
                if (!manager.ReceivedStatus)
                {
                    //TODO: proper deregistration handling
                    context.TaskManagers.Remove(manager);
                    Logger.Error($"FAILURE - task manager with id {manager.Id}");
                }
                else
                {
                    manager.ReceivedStatus = false;
                }
            }
        }
    }
}
