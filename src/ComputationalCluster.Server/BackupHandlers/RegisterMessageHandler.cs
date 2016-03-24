﻿using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System.Linq;

namespace ComputationalCluster.Server.BackupHandlers
{
    public class RegisterMessageHandler : IResponseHandler<RegisterMessage>
    {
        private readonly IServerContext context;

        public RegisterMessageHandler(IServerContext context)
        {
            this.context = context;
        }

        public void HandleResponse(RegisterMessage message)
        {
            context.BackupMessages.Enqueue(message);
            switch (message.Type.Type)
            {
                case ClientComponentType.ComputationalNode:
                    HandleNode(message);
                    break;
                case ClientComponentType.TaskManager:
                    HandleTaskManager(message);
                    break;
            }
        }

        private void HandleTaskManager(RegisterMessage message)
        {
            if (message.DeregisterSpecified && message.Deregister)
            {
                DeregisterTaskManager(message);
            }
            else
            {
                RegisterTaskManager(message);
            }
        }

        private void RegisterTaskManager(RegisterMessage message)
        {
            var taskManager = new TaskManager
            {
                Id = (int)message.Id,
                SolvableProblems = message.SolvableProblems,
                ThreadsCount = message.ParallelThreads
            };
            context.TaskManagers.Add(taskManager);
        }

        private void DeregisterTaskManager(RegisterMessage message)
        {
            var taskManager = context.TaskManagers.FirstOrDefault(t => t.Id == (int)message.Id);
            if (taskManager != null)
            {
                context.TaskManagers.Remove(taskManager);
            }
        }

        private void HandleNode(RegisterMessage message)
        {
            if (message.DeregisterSpecified && message.Deregister)
            {
                DeregisterNode(message);
            }
            else
            {
                RegisterNode(message);
            }
        }

        private void RegisterNode(RegisterMessage message)
        {
            var node = new ComputationalNode
            {
                Id = (int)message.Id,
                SolvableProblems = message.SolvableProblems,
                ThreadsCount = message.ParallelThreads
            };
            context.Nodes.Add(node);
        }

        private void DeregisterNode(RegisterMessage message)
        {
            var node = context.Nodes.FirstOrDefault(n => n.Id == (int)message.Id);
            if (node != null)
            {
                context.Nodes.Remove(node);
                foreach (var problem in context.Problems)
                {
                    foreach (var partial in problem.PartialProblems)
                    {
                        if (partial.NodeId == node.Id)
                        {
                            partial.State = PartialProblemState.New;
                        }
                    }
                }
            }
        }
    }
}