using System.Collections.Generic;
using System.Linq;
using ComputationalCluster.DVRPTaskSolver.Parsing;
using ComputationalCluster.DVRPTaskSolver.Problem;

namespace ComputationalCluster.DVRPTaskSolver.Algorithm
{
    public class ProblemDivider
    {
        DVRPProblemInstance problem;
        List<Partition> partitions;
        int threadCount;

        public ProblemDivider(DVRPProblemInstance problem, int threadCount)
        {
            this.threadCount = threadCount;
            this.problem = problem;
            partitions = new List<Partition>();
            
        }

        private void CreatePartitions()
        {
            var numbers = Enumerable.Range(1, problem.Clients.Length);
            foreach (var partition in GeneratePartitions(numbers.ToArray(), new List<int>(), problem.VehiclesCount))
            {
                Partition p = new Partition(problem.VehiclesCount);
                p.truckClients = partition;
                partitions.Add(p);
            }
        }

        private IEnumerable<List<int>[]> GeneratePartitions(int[] numbers, List<int> currentSet, int subsets)
        {
            if (currentSet.Count == numbers.Length)
            {
                var partition = new List<int>[subsets];
                for (int i = 0; i < subsets; i++)
                {
                    partition[i] = new List<int>();
                }
                for (int i = 0; i < currentSet.Count; i++)
                {
                    int subsetIndex = currentSet[i];
                    partition[subsetIndex].Add(numbers[i]);
                }
                yield return partition;
            }
            else
            {
                for (int i = 0; i < subsets; i++)
                {
                    foreach (var partition in GeneratePartitions(numbers,
                        currentSet.Concat(new List<int> { i }).ToList(), subsets))
                    {
                        yield return partition;
                    }
                }
            }
        }

        public List<Partition>[] DividePartitions()
        {
            List<Partition>[] ret = new List<Partition>[threadCount];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = new List<Partition>();
            }

            for(int i = 0; i< partitions.Count; i++)
            {
                ret[i % threadCount].Add(partitions[i]);
            }

            return ret;
        }
    }
}
