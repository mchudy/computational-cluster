using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComputationalCluster.Common.Objects
{
    [Serializable()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public class StatusThread
    {
        public StatusThreadState State { get; set; }

        public ulong HowLong { get; set; }

        [XmlIgnore()]
        public bool HowLongSpecified { get; set; }

        public ulong ProblemInstanceId { get; set; }

        [XmlIgnore()]
        public bool ProblemInstanceIdSpecified { get; set; }

        public ulong TaskId { get; set; }

        [XmlIgnore()]
        public bool TaskIdSpecified { get; set; }

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
