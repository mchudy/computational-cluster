using System.IO;
using System.Text;

namespace ComputationalCluster.Common.Utils
{
    public sealed class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}
