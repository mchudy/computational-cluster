using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;

namespace ComputationalCluster.TaskManager
{
    public class TaskManager
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TaskManager));

        private readonly IMessenger messenger;
        private readonly TaskManagerContext context;

        public TaskManager(IMessenger messenger, TaskManagerContext context)
        {
            this.messenger = messenger;
            this.context = context;
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
                messenger.SendMessage(message);
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }
        }
    }
}
