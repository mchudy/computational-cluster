using ComputationalCluster.Common.Objects;
using System.Collections.Generic;

namespace ComputationalCluster.Client
{
    public class ClientContext
    {
        public int WaitTime { get; set; } = 4;
        public IList<BackupCommunicationServer> BackupServers { get; set; } = new List<BackupCommunicationServer>();
        public int? CurrentProblemId { get; set; }
    }
}
