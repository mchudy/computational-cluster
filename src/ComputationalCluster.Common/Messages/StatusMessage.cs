using ComputationalCluster.Common.Objects;
using System;
using System.Xml.Serialization;

namespace ComputationalCluster.Common.Messages
{
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [XmlRoot(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false, ElementName = "Status")]
    public class StatusMessage
    {
        public ulong Id { get; set; }

        [XmlArrayItem("Thread", IsNullable = false)]
        public StatusThread[] Threads { get; set; }
    }
}
