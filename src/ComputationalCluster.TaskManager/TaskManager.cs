using Autofac;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.TaskManager
{
    public class TaskManager
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TaskManager));

        private readonly IMessenger messenger;
        private readonly TaskManagerContext context;

        //TODO!
        private readonly IComponentContext componentContext;
        private uint timeout;

        public TaskManager(IMessenger messenger, TaskManagerContext context, IComponentContext componentContext)
        {
            this.messenger = messenger;
            this.context = context;
            this.componentContext = componentContext;
        }

        public void Start()
        {
            Register();
        }

        private void Register()
        {
            var message = new RegisterMessage()
            {
                SolvableProblems = new[] { "DVRP" },
                ParallelThreads = TaskManagerContext.ParallelThreads,
                Type = RegisterType.TaskManager
            };
            try
            {
                IList<Message> responses = messenger.SendMessage(message);
                var responseMessage = responses[0] as RegisterResponseMessage;
                if (responseMessage != null)
                {
                    var response = responseMessage;
                    timeout = response.Timeout;
                    context.Id = (int)response.Id;
                    logger.Info($"Registered with id {context.Id}");
                }
                Task.Run(() => SendStatus());
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }

        private void SendStatus()
        {
            while (true)
            {
                try
                {
                    logger.Debug("Sending status");
                    var statusMessage = GetStatus();
                    var response = messenger.SendMessage(statusMessage);
                    Task.Run(() => HandleResponse(response));
                    Thread.Sleep((int)(timeout * 1000 / 2));
                }
                catch (SocketException)
                {
                    logger.Error("Server failure");
                }
            }
        }

        private void HandleResponse(IList<Message> response)
        {
            foreach (var message in response)
            {
                HandleMessage(message);
            }
        }

        //TODO: move
        private void HandleMessage(Message message)
        {
            var type = typeof(IResponseHandler<>).MakeGenericType(message.GetType());
            var handler = componentContext.Resolve(type);
            type.InvokeMember(nameof(IResponseHandler<SolutionMessage>.HandleResponse), BindingFlags.InvokeMethod, null, handler,
                new object[] { message });
        }

        private StatusMessage GetStatus()
        {
            var statusMessage = new StatusMessage
            {
                Id = (ulong)context.Id,
                Threads = context.Threads
            };
            return statusMessage;
        }
    }
}
