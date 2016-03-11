using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Common.Objects
{
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public class BackupCommunicationServer
    {
        [System.Xml.Serialization.XmlAttribute(DataType = "anyURI")]
        public string Address { get; set; }

        [System.Xml.Serialization.XmlAttribute()]
        public ushort Port { get; set; }
    }
}
