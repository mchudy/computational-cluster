﻿using ComputationalCluster.Common.Objects;
using System;
using System.Xml.Serialization;

namespace ComputationalCluster.Common.Messages
{
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [XmlRoot(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false, ElementName = "Solutions")]
    public class SolutionMessage : Message
    {
        public string ProblemType { get; set; }

        public ulong Id { get; set; }

        [XmlElement(DataType = "base64Binary")]
        public byte[] CommonData { get; set; }

        [XmlArrayItem("Solution")]
        public Solution[] Solutions { get; set; }
    }
}
