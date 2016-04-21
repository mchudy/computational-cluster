using ComputationalCluster.DVRPTaskSolver.Algorithm;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ComputationalCluster.DVRPTaskSolver.Problem
{
    public class PartialProblemsSerializer
    {
        public byte[][] Serialize(DVRPProblemInstance problemInstance, List<Partition>[] partitions)
        {
            byte[][] result = new byte[partitions.Length][];
            for (int i = 0; i < partitions.Length; i++)
            {
                var partialProblem = new DVRPPartialProblem
                {
                    ProblemInstance = problemInstance,
                    Partitions = partitions[i]
                };
                var xmlSerializer = new XmlSerializer(typeof(DVRPPartialProblem));
                string xml;
                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, partialProblem);
                    xml = textWriter.ToString();
                }
                result[i] = Encoding.UTF8.GetBytes(xml);
            }
            return result;
        }

        public DVRPPartialProblem Deserialize(byte[] data)
        {
            var xml = Encoding.UTF8.GetString(data);
            var xmlSerializer = new XmlSerializer(typeof(DVRPPartialProblem));
            DVRPPartialProblem partialProblem;
            using (var reader = new StringReader(xml))
            {
                partialProblem = (DVRPPartialProblem)xmlSerializer.Deserialize(reader);
            }
            return partialProblem;
        }
    }
}
