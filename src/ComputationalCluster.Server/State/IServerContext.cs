using ComputationalCluster.Common.Messages;
using ComputationalCluster.Server.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ComputationalCluster.Server
{
    public interface IServerContext
    {
        List<BackupServer> BackupServers { get; set; }
        IServerConfiguration Configuration { get; }
        IList<ComputationalNode> Nodes { get; }
        IList<ProblemInstance> Problems { get; }
        IList<TaskManager> TaskManagers { get; }
        bool IsPrimary { get; }
        int Id { get; set; }
        int GetNextComponentId();
        int GetNextProblemId();
        ConcurrentQueue<Message> BackupMessages { get; }
        NoOperationMessage GetNoOperationMessage();

    }
}