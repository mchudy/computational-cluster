using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Common.Objects
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34283")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public class Solution
    {
        public ulong TaskId { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TaskIdSpecified { get; set; }

        public bool TimeoutOccured { get; set; }

        public SolutionType Type { get; set; }

        public ulong ComputationsTime { get; set; }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] Data { get; set; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34283")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public enum SolutionType
    {
        Ongoing,
        Partial,
        Final,
    }
}
