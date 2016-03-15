using System.IO;
using System.Text;

namespace ComputationalCluster.Server.Extensions
{
    public static class StreamReaderExtensions
    {
        public static string ReadToChar(this StreamReader sr, char splitCharacter)
        {
            StringBuilder line = new StringBuilder();
            while (sr.Peek() > 0)
            {
                var nextChar = (char)sr.Read();
                if (nextChar == splitCharacter) return line.ToString();
                line.Append(nextChar);
            }
            return line.Length == 0 ? null : line.ToString();
        }
    }
}
