using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
using System.Threading;

namespace ComputationalCluster.TaskManager.Handlers
{
    public class SolutionMessageHandler : IResponseHandler<SolutionMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SolutionMessageHandler));

        private readonly TaskManagerContext context;
        private readonly IMessenger messenger;

        public SolutionMessageHandler(TaskManagerContext context, IMessenger messenger)
        {
            this.context = context;
            this.messenger = messenger;
        }

        public void HandleResponse(SolutionMessage message)
        {
            var idleThread = context.TakeThread();
            if (idleThread != null)
            {
                idleThread.State = StatusThreadState.Busy;
                idleThread.TaskId = message.Id;
                //TODO:
                Thread.Sleep(5000);
                messenger.SendMessage(new SolutionMessage
                {
                    Id = message.Id,
                    ProblemType = message.ProblemType,
                    Solutions = new[]
                    {
                        new Solution
                        {
                            TaskId = message.Id,
                            Type = SolutionType.Final
                        }
                    }
                });
                context.ReleaseThread(idleThread);
            }
            else
            {
                logger.Error("No idle thread available");
                //TODO: send error message
            }
        }
    }
}
