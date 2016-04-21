using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ComputationalCluster.DVRPTaskSolver.Problem
{
    public class SolutionsSerializer
    {
        public DVRPSolution[] Deserialize(byte[][] solutionsData)
        {
            var solutions = new DVRPSolution[solutionsData.Length];
            for (int i = 0; i < solutionsData.Length; i++)
            {
                var xml = Encoding.UTF8.GetString(solutionsData[i]);
                var xmlSerializer = new XmlSerializer(typeof(DVRPSolution));
                using (var reader = new StringReader(xml))
                {
                    solutions[i] = (DVRPSolution)xmlSerializer.Deserialize(reader);
                }
            }
            return solutions;
        }


        public byte[] Serialize(DVRPSolution solution)
        {
            var xmlSerializer = new XmlSerializer(typeof(DVRPSolution));
            string xml;
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, solution);
                xml = textWriter.ToString();
            }
            return Encoding.UTF8.GetBytes(xml);
        }

        public byte[] SerializeForClient(DVRPSolution finalSolution)
        {
            //TODO: better format?
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < finalSolution.Routes.Length; i++)
            {
                foreach (var stop in finalSolution.Routes[i])
                {
                    builder.Append(stop + ",");
                }
                builder.AppendLine();
            }
            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}
