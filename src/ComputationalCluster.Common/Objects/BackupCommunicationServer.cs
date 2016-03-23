using System;

namespace ComputationalCluster.Common.Objects
{
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public class BackupCommunicationServer
    {
        public BackupCommunicationServer()
        { }

        public BackupCommunicationServer(string address, ushort port)
        {
            Address = address;
            Port = port;
        }

        [System.Xml.Serialization.XmlAttribute(DataType = "anyURI")]
        public string Address { get; set; }

        [System.Xml.Serialization.XmlAttribute()]
        public ushort Port { get; set; }

        public int Id { get; set; }
    }
}
