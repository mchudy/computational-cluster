﻿using System;
using System.Collections.Generic;

namespace ComputationalCluster.DVRPTaskSolver.Algorithm
{
    [Serializable]
    public class Partition
    {
        public List<int>[] truckClients;
        public Partition(int trucks)
        {
            truckClients = new List<int>[trucks];
            for (int i = 0; i < truckClients.Length; i++)
                truckClients[i] = new List<int>();
        }

    }
}