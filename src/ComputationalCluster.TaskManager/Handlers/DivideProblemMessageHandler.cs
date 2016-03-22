using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
using System.Threading;

namespace ComputationalCluster.TaskManager
{
    public class DivideProblemMessageHandler : IResponseHandler<DivideProblemMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DivideProblemMessageHandler));

        private readonly TaskManagerContext context;
        private readonly IMessenger messenger;

        public DivideProblemMessageHandler(TaskManagerContext context, IMessenger messenger)
        {
            this.context = context;
            this.messenger = messenger;
        }

        public void HandleResponse(DivideProblemMessage message)
        {
            var idleThread = context.TakeThread();
            if (idleThread != null)
            {
                idleThread.State = StatusThreadState.Busy;
                idleThread.ProblemInstanceId = message.Id;
                idleThread.ProblemType = message.ProblemType;
                //TODO:
                Thread.Sleep(5000);
                messenger.SendMessage(new PartialProblemsMessage
                {
                    Id = message.Id,
                    ProblemType = message.ProblemType,
                    PartialProblems = new[]
                    {
                        new PartialProblem
                        {
                            TaskId = 0,
                            NodeID = (ulong) context.Id
                        },
                        new PartialProblem
                        {
                            TaskId = 1,
                            NodeID = (ulong) context.Id
                        }
                    }
                });
                logger.Info($"Sending partial problems for problem {message.Id}");
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
