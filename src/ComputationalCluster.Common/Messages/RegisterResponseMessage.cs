using ComputationalCluster.Common.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComputationalCluster.Common.Messages
{
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [XmlRoot(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false)]
    public class RegisterResponseMessage
    {
        public ulong Id { get; set; }

        public uint Timeout { get; set; }

        public IList<BackupCommunicationServer> BackupCommunicationServers { get; set; }
    }
}
