using ComputationalCluster.Common.Objects;
using ComputationalCluster.Server.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace ComputationalCluster.Server
{
    public class ServerContext : IServerContext
    {
        private int currentProblemId;
        private int currentComponentId;

        public ServerContext(IServerConfiguration configuration)
        {
            Configuration = configuration;
        }

        //TODO: dictionary for fast access?

        public IServerConfiguration Configuration { get; }

        public IList<TaskManager> TaskManagers { get; } = new List<TaskManager>();
        public IList<ComputationalNode> Nodes { get; } = new List<ComputationalNode>();
        public IList<ProblemInstance> Problems { get; } = new List<ProblemInstance>();
        public IList<BackupServer> BackupServers { get; } = new List<BackupServer>();

        public ConcurrentQueue<ProblemInstance> ProblemQueue { get; } = new ConcurrentQueue<ProblemInstance>();

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
        public bool ReceivedStatus { get; set; }
    }

    public class BackupServer : ClientComponent
    {
        public string Address { get; set; }
        public ushort Port { get; set; }
    }

    public class ProblemInstance
    {
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public ulong SolvingTimeout { get; set; }
        public string ProblemType { get; set; }
        public ProblemStatus Status { get; set; }
        public PartialProblem[] PartialProblems { get; set; }
    }

    public enum ProblemStatus
    {
        New,
        Dividing,
        Divided,
        ComputationOngoing,
        Partial,
        Merging,
        Final
    }
}
