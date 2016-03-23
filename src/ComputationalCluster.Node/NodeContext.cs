using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Objects;
using System.Collections.Generic;

namespace ComputationalCluster.Node
{
    public class NodeContext
    {
        public const int ParallelThreads = 3;

        public NodeContext()
        {
            InitializeThreads();
        }

        public string CurrentProblemType { get; set; }
        public int CurrentId { get; set; }
        public ulong SolvingTimeout { get; set; }
        public IList<PartialProblem> CurrentPartialProblems { get; set; }
        public IList<Solution> CurrentSolutions { get; set; }
        public uint Timeout { get; set; }
        public int Id { get; set; }
        public StatusThread[] Threads { get; } = new StatusThread[ParallelThreads];
        public IList<BackupCommunicationServer> BackupServers { get; set; }

        public StatusMessage GetStatus()
        {
            return new StatusMessage
            {
                Id = (ulong)Id,
                Threads = Threads
            };
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
    }
}
