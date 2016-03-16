using System.Collections.Generic;
using System.Threading;

namespace ComputationalCluster.Server
{
    public class ServerContext
    {
        private int currentProblemId;
        private int currentComponentId;

        public IList<TaskManager> TaskManagers { get; } = new List<TaskManager>();
        public IList<ComputationalNode> Nodes { get; } = new List<ComputationalNode>();
        public IList<ProblemInstance> Problems { get; } = new List<ProblemInstance>();
        public IList<BackupServer> BackupServers { get; } = new List<BackupServer>();

        public int GetNextComponentId()
        {
            return Interlocked.Increment(ref currentComponentId);
        }

        public int GetNextProblemId()
        {
            return Interlocked.Increment(ref currentProblemId);
        }
    }

    public abstract class ClientComponent
    {
        public int Id { get; set; }
    }

    public class TaskManager : ClientComponent
    {
        public IList<string> SolvableProblems { get; set; } = new List<string>();
        public int ThreadsCount { get; set; }
        public bool ReceivedStatus { get; set; }
    }

    public class ComputationalNode : ClientComponent
    {
        public IList<string> SolvableProblems { get; set; } = new List<string>();
        public int ThreadsCount { get; set; }
    }

    public class BackupServer : ClientComponent
    {

    }

    public class ProblemInstance
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public ulong SolvingTimeout { get; set; }
        public string ProblemType { get; set; }

    }
}
