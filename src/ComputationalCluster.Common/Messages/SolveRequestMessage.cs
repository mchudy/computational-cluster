﻿using System.Xml.Serialization;

namespace ComputationalCluster.Common.Messages
{
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThrough()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [XmlRoot(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false, ElementName = "SolveRequest")]
    public class SolveRequestMessage : Message
    {
        public string ProblemType { get; set; }

        public ulong SolvingTimeout { get; set; }

        [XmlIgnore()]
        public bool SolvingTimeoutSpecified { get; set; }

        [XmlElement(DataType = "base64Binary")]
        public byte[] Data { get; set; }

        public ulong Id { get; set; }

        [XmlIgnore()]
        public bool IdSpecified { get; set; }
    }
}
