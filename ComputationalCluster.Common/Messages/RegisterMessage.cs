﻿using System;
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
    public class RegisterMessage
    {
        public RegisterType Type { get; set; }

        [XmlArrayItem("ProblemName", IsNullable = false)]
        public string[] SolvableProblems { get; set; }

        public byte ParallelThreads { get; set; }

        [XmlIgnore()]
        public bool DeregisterSpecified { get; set; }

        public bool Deregister { get; set; }

        [XmlIgnore()]
        public bool IdSpecified { get; set; }

        public ulong Id { get; set; }

    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public enum RegisterType
    {
        TaskManager,
        ComputationalNode,
        CommunicationServer,
    }
}



