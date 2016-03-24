using System;
using System.Xml.Serialization;

namespace ComputationalCluster.Common.Objects
{
    [Serializable()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public class StatusThread
    {
        public StatusThreadState State { get; set; }

        public ulong? HowLong { get; set; }
        public bool ShouldSerializeHowLong() => HowLong != null;

        public ulong? ProblemInstanceId { get; set; }
        public bool ShouldSerializeProblemInstanceId() => ProblemInstanceId != null;

        public ulong? TaskId { get; set; }
        public bool ShouldSerializeTaskId() => TaskId != null;

        public string ProblemType { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public enum StatusThreadState
    {
        Idle,
        Busy
    }
}
