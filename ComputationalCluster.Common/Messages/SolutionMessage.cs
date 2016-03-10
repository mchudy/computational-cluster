using ComputationalCluster.Common.Objects;
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
    public class SolutionMessage
    {
        public string ProblemType { get; set; }

        public ulong Id { get; set; }

        [XmlElement(DataType = "base64Binary")]
        public byte[] CommonData { get; set; }

        [XmlArrayItem("Solution", IsNullable = false)]
        public Solution[] Solutions { get; set; }
    }
}
