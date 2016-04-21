using ComputationalCluster.DVRPTaskSolver.Algorithm;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

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
                string json = JsonConvert.SerializeObject(partialProblem);
                result[i] = Encoding.UTF8.GetBytes(json);
            }
            return result;
        }

        public DVRPPartialProblem Deserialize(byte[] data)
        {
            var json = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<DVRPPartialProblem>(json);
        }
    }
}
