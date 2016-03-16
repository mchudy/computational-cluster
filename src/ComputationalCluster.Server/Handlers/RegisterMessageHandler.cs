using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using ComputationalCluster.Server.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
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

        public void HandleMessage(RegisterMessage message, NetworkStream stream)
        {
            int id = context.GetNextComponentId();
            Console.WriteLine("Received register message");
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
                Console.WriteLine($"New task manager registered - id {id}");
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
                Console.WriteLine($"New backup server registered - id {id}");
                context.BackupServers.Add(new BackupServer
                {
                    Id = id
                });
            }
            var responseMessage = new RegisterResponseMessage
            {
                Id = (ulong)id,
                Timeout = configuration.Timeout,
                BackupCommunicationServers = new List<BackupCommunicationServer>()
            };
            messenger.SendMessage(responseMessage, stream);
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
                    Console.WriteLine($"FAILURE - task manager with id {manager.Id}");
                }
                else
                {
                    manager.ReceivedStatus = false;
                }
            }
        }
    }
}
