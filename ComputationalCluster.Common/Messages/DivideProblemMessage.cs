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
    public class DivideProblemMessage
    {
        public string ProblemType { get; set; }

        public ulong Id { get; set; }

        [XmlElement(DataType = "base64Binary")]
        public byte[] Data { get; set; }

        public ulong ComputationalNodes { get; set; }

        public ulong NodeID { get; set; }
    }
}
