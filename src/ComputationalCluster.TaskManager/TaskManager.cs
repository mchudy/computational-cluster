using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ComputationalCluster.TaskManager
{
    public class TaskManager
    {
        private const int parallelThreads = 8;

        //TODO: custom class?
        private readonly StatusThread[] threads = new StatusThread[parallelThreads];
        private readonly IConfiguration configuration;
        private readonly IMessenger messenger;
        private uint timeout;
        private ulong id;

        public TaskManager(IConfiguration configuration, IMessenger messenger)
        {
            this.configuration = configuration;
            this.messenger = messenger;
        }

        public void Start()
        {
            InitializeThreads();
            Register();
        }

        private void InitializeThreads()
        {
            for (int i = 0; i < parallelThreads; i++)
            {
                threads[i] = new StatusThread
                {
                    State = StatusThreadState.Idle
                };
            }
        }

        private void Register()
        {
            var message = new RegisterMessage()
            {
                SolvableProblems = new[] { "DVRP" },
                ParallelThreads = parallelThreads,
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
                    id = response.Id;
                    Console.WriteLine($"Registered with id {id}");
                }
                Task.Run(() => SendStatus());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void SendStatus()
        {
            while (true)
            {
                Console.WriteLine("Sending status");
                var statusMessage = GetStatus();
                var response = messenger.SendMessage(statusMessage);
                HandleResponse(response);
                Thread.Sleep((int)(timeout * 1000));
            }
        }

        private void HandleResponse(IList<Message> response)
        {
            var message = response[0];
            if (message is NoOperationMessage)
            {
                message = (NoOperationMessage)message;
            }

        }

        private StatusMessage GetStatus()
        {
            var statusMessage = new StatusMessage
            {
                Id = id,
                Threads = threads
            };
            return statusMessage;
        }
    }
}
