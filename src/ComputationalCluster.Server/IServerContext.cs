﻿using ComputationalCluster.Server.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ComputationalCluster.Server
{
    public interface IServerContext
    {
        IList<BackupServer> BackupServers { get; }
        IServerConfiguration Configuration { get; }
        IList<ComputationalNode> Nodes { get; }
        ConcurrentQueue<ProblemInstance> ProblemQueue { get; }
        IList<ProblemInstance> Problems { get; }
        IList<TaskManager> TaskManagers { get; }

        int GetNextComponentId();
        int GetNextProblemId();
    }
}