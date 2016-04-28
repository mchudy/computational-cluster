using System;
using System.Collections.Generic;
using System.Linq;

namespace ComputationalCluster.DVRPTaskSolver.Algorithm
{
    public class Partition
    {
        public List<int>[] truckClients;

        public Partition()
        {

        }

        public Partition(int trucks)
        {
            truckClients = new List<int>[trucks];
            for (int i = 0; i < truckClients.Length; i++)
                truckClients[i] = new List<int>();
        }

        public IEnumerable<List<int>[]> MakePartitions(int items)
        {
            if (items < 1)
                yield break;
            var currentPartition = new int[items];
            do
            {
                var groups = new List<int>[currentPartition.Max() + 1];
                for (int i = 0; i < currentPartition.Length; ++i)
                {
                    int groupIndex = currentPartition[i];
                    if (groups[groupIndex] == null)
                        groups[groupIndex] = new List<int>();
                    groups[groupIndex].Add(i+1);
                }
                yield return groups;
            } while (NextPartition(currentPartition));
        }

        private bool NextPartition(int[] currentPartition)
        {
            int index = currentPartition.Length - 1;
            while (index >= 0)
            {
                ++currentPartition[index];
                if (Valid(currentPartition))
                    return true;
                currentPartition[index--] = 0;
            }
            return false;
        }

        private bool Valid(int[] currentPartition)
        {
            var uniqueSymbolsSeen = new HashSet<int>();
            foreach (var item in currentPartition)
            {
                uniqueSymbolsSeen.Add(item);
                if (uniqueSymbolsSeen.Count <= item)
                    return false;
            }
            return true;
        }
    }


}
