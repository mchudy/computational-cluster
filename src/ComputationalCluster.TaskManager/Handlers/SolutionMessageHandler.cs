using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
using System.Linq;

namespace ComputationalCluster.TaskManager.Handlers
{
    public class SolutionMessageHandler : IResponseHandler<SolutionMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SolutionMessageHandler));

        private readonly TaskManagerContext context;
        private readonly IMessenger messenger;
        private readonly ITaskSolverProvider taskSolverProvider;

        public SolutionMessageHandler(TaskManagerContext context, IMessenger messenger,
            ITaskSolverProvider taskSolverProvider)
        {
            this.context = context;
            this.messenger = messenger;
            this.taskSolverProvider = taskSolverProvider;
        }

        public void HandleResponse(SolutionMessage message)
        {
            var idleThread = context.TakeThread();
            if (idleThread != null)
            {
                idleThread.State = StatusThreadState.Busy;
                idleThread.TaskId = message.Id;

                //TODO: run on separate thread
                var taskSolver = taskSolverProvider.CreateTaskSolverInstance(message.ProblemType, null);
                byte[] finalSolution = taskSolver.MergeSolution(
                    message.Solutions.Select(s => s.Data)
                        .ToArray()
                );

                messenger.SendMessage(new SolutionMessage
                {
                    Id = message.Id,
                    ProblemType = message.ProblemType,
                    Solutions = new[]
                    {
                        new Solution
                        {
                            TaskId = message.Id,
                            Type = SolutionType.Final,
                            Data = finalSolution
                        }
                    }
                });
                logger.Info($"Sending final solution for problem {message.Id}");
                context.ReleaseThread(idleThread);
            }
            else
            {
                logger.Error("No idle thread available");
                messenger.SendMessage(new ErrMessage { ErrorType = ErrorErrorType.ExceptionOccured });
            }
        }
    }
}
