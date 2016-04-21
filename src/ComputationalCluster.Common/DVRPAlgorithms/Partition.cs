using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Common.DVRPAlgorithms
{
    public class Partition
    {
        public List<int>[] truckClients;
        public Partition(int trucks)
        {
            truckClients = new List<int>[trucks];
            for(int i =0; i< truckClients.Length; i++)
                truckClients[i] = new List<int>();
        }
        
    }
}
