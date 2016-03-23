using System;
using System.Xml.Serialization;

namespace ComputationalCluster.Common.Messages
{
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [XmlRoot(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false, ElementName = "Register")]
    public class RegisterMessage : Message
    {
        public ComponentType Type { get; set; }

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

    [Serializable]
    [System.ComponentModel.DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public class ComponentType
    {

        [XmlAttribute(AttributeName = "port")]
        public ushort port { get; set; }
        [XmlText()]
        public ClientComponentType Type { get; set; }
    }

    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public enum ClientComponentType
    {
        TaskManager,
        ComputationalNode,
        CommunicationServer,
    }
}




