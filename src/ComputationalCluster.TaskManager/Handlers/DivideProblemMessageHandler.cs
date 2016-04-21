using ComputationalCluster.Common;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Objects;
using log4net;
using System;
using System.Threading.Tasks;

namespace ComputationalCluster.TaskManager
{
    public class DivideProblemMessageHandler : IResponseHandler<DivideProblemMessage>
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DivideProblemMessageHandler));

        private readonly TaskManagerContext context;
        private readonly IMessenger messenger;
        private readonly ITaskSolverProvider taskSolverProvider;

        public DivideProblemMessageHandler(TaskManagerContext context, IMessenger messenger,
            ITaskSolverProvider taskSolverProvider)
        {
            this.context = context;
            this.messenger = messenger;
            this.taskSolverProvider = taskSolverProvider;
        }

        public void HandleResponse(DivideProblemMessage message)
        {
            var idleThread = context.TakeThread();
            if (idleThread != null)
            {
                Task.Run(() => DivideProblem(message, idleThread));
            }
            else
            {
                logger.Error("No idle thread available");
                messenger.SendMessage(new ErrMessage { ErrorType = ErrorErrorType.ExceptionOccured });
            }
        }

        private void DivideProblem(DivideProblemMessage message, StatusThread idleThread)
        {
            logger.Info($"Dividing problem {message.Id}");
            idleThread.ProblemInstanceId = message.Id;
            idleThread.ProblemType = message.ProblemType;
            try
            {
                var taskSolver = taskSolverProvider.CreateTaskSolverInstance(message.ProblemType, message.Data);
                var partialProblemsData = taskSolver.DivideProblem((int)message.ComputationalNodes);

                var partialProblems = new PartialProblem[message.ComputationalNodes];
                for (int i = 0; i < partialProblems.Length; i++)
                {
                    partialProblems[i] = new PartialProblem
                    {
                        TaskId = (ulong)i,
                        NodeID = (ulong)context.Id,
                        Data = partialProblemsData[i]
                    };
                }
                messenger.SendMessage(new PartialProblemsMessage
                {
                    Id = message.Id,
                    ProblemType = message.ProblemType,
                    PartialProblems = partialProblems
                });
                logger.Info($"Sending partial problems for problem {message.Id}");
            }
            catch (Exception e)
            {
                logger.Error(e.StackTrace);
                messenger.SendMessage(new ErrMessage { ErrorType = ErrorErrorType.ExceptionOccured });
            }
            finally
            {
                context.ReleaseThread(idleThread);
            }
        }
    }
}
