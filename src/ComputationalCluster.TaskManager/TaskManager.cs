using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<BackupCommunicationServer> backupServers;

        public TaskManager(IConfiguration configuration, IMessenger messenger)
        {
            this.configuration = configuration;
            this.messenger = messenger;
        }

        public ILog Logger { get; set; }

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
                    Logger.Info($"Registered with id {id}");
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
                Logger.Debug("Sending status");
                var statusMessage = GetStatus();
                var response = messenger.SendMessage(statusMessage);
                Task.Run(() => HandleResponse(response));
                Thread.Sleep((int)(timeout * 1000 / 2));
            }
        }

        private void HandleResponse(IList<Message> response)
        {
            Message message = response[0];
            if (message is NoOperationMessage)
            {
                this.backupServers = ((NoOperationMessage)message).BackupCommunicationServers;
            }
            else if (message is DivideProblemMessage)
            {
                var divideMessage = (DivideProblemMessage)message;
                var idleThread = threads.FirstOrDefault(t => t.State == StatusThreadState.Idle);
                if (idleThread != null)
                {
                    idleThread.State = StatusThreadState.Busy;
                    idleThread.ProblemInstanceId = divideMessage.Id;
                    idleThread.ProblemType = divideMessage.ProblemType;
                    //TODO:
                    Thread.Sleep(5000);
                    messenger.SendMessage(new PartialProblemsMessage
                    {
                        Id = divideMessage.Id
                    });
                    ReleaseThread(idleThread);
                }
                else
                {
                    Logger.Error("No idle thread available");
                    //TODO: send error message
                }
            }
        }

        private void ReleaseThread(StatusThread idleThread)
        {
            idleThread.ProblemInstanceId = null;
            idleThread.HowLong = null;
            idleThread.State = StatusThreadState.Idle;
            idleThread.ProblemType = null;
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
