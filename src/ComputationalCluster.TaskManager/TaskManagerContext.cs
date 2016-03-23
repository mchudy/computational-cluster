using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Objects;
using System.Collections.Generic;
using System.Linq;

namespace ComputationalCluster.TaskManager
{
    //TODO: extract threads logic and make common with node
    public class TaskManagerContext
    {
        private static readonly object lockObject = new object();

        public TaskManagerContext()
        {
            InitializeThreads();
        }

        private void InitializeThreads()
        {
            for (int i = 0; i < ParallelThreads; i++)
            {
                Threads[i] = new StatusThread
                {
                    State = StatusThreadState.Idle
                };
            }
        }

        public const int ParallelThreads = 3;
        public int Id { get; set; }
        public int Timeout { get; set; }
        public StatusThread[] Threads { get; } = new StatusThread[ParallelThreads];
        public IList<BackupCommunicationServer> BackupServers { get; set; } = new List<BackupCommunicationServer>();

        public StatusThread TakeThread()
        {
            lock (lockObject)
            {
                var idleThread = Threads.FirstOrDefault(t => t.State == StatusThreadState.Idle);
                if (idleThread != null)
                {
                    idleThread.State = StatusThreadState.Busy;
                }
                return idleThread;
            }
        }

        public void ReleaseThread(StatusThread idleThread)
        {
            lock (lockObject)
            {
                idleThread.ProblemInstanceId = null;
                idleThread.HowLong = null;
                idleThread.State = StatusThreadState.Idle;
                idleThread.ProblemType = null;
            }
        }

        public StatusMessage GetStatus()
        {
            var statusMessage = new StatusMessage
            {
                Id = (ulong)Id,
                Threads = Threads
            };
            return statusMessage;
        }
    }
}
