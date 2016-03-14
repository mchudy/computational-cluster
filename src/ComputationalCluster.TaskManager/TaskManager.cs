using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using System;

namespace ComputationalCluster.TaskManager
{
    public class TaskManager
    {
        private readonly IConfiguration configuration;
        private readonly IMessenger messenger;

        public TaskManager(IConfiguration configuration, IMessenger messenger)
        {
            this.configuration = configuration;
            this.messenger = messenger;
        }

        public void Start()
        {
            var message = new ErrorMessage
            {
                ErrorType = ErrorErrorType.ExceptionOccured
            };
            try
            {
                messenger.SendMessageAndClose(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
