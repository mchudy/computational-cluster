using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComputationalCluster.Common.Objects
{
    [Serializable()]
    [System.Diagnostics.DebuggerStepThrough()]
    [XmlType(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public class PartialProblem
    {
        public ulong TaskId { get; set; }

        [XmlElement(DataType = "base64Binary")]
        public byte[] Data { get; set; }

        public ulong NodeID { get; set; }
    }
}
