using ComputationalCluster.DVRPTaskSolver.Problem;
using System;
using System.Collections.Generic;

namespace ComputationalCluster.DVRPTaskSolver.Algorithm
{
    public class DVRPSolver
    {
        private DVRPPartialProblem partialProblem;
        private DVRPProblemInstance problem;
        double minCost;
        List<Partition> partitions;

        public DVRPSolver(DVRPPartialProblem partialProblem)
        {
            this.partialProblem = partialProblem;
            this.problem = partialProblem.ProblemInstance;
            minCost = double.MaxValue;
        }

        public DVRPSolution Solve()
        {
            throw new NotImplementedException();
        }

        private double TravelDistance(Location l1, Location l2)
        {
            return Math.Sqrt((l1.X - l2.X) * (l1.X - l2.X) + (l1.Y - l2.Y) * (l1.Y - l2.Y));
        }

        double CheckTimesCapacityAndCost(List<int> route)
        {
            for (int i = 0; i < route.Count; i++)
            {

            }
            return 0;
        }


        void Generate(int k, bool zDepot, List<int> permutation, List<List<int>> ret, bool[] tab, bool flag)
        {
            if (k == problem.Clients.Length)
            {
                permutation.Add(0);
                if (flag)
                {
                    ret.Add(permutation);
                    flag = false;
                }
                else
                    flag = true;
                return;
            }

            for (int m = 1; m < problem.Clients.Length + 1; ++m)
            {
                var p = new List<int>(permutation);
                if (!tab[m - 1])
                {
                    tab[m - 1] = true;

                    if (zDepot || k == 0)
                        p.Add(0);

                    p.Add(m);
                    Generate(k + 1, true, new List<int>(p), ret, tab, flag);
                    Generate(k + 1, false, new List<int>(p), ret, tab, flag);

                    tab[m - 1] = false;
                }
            }

        }

        List<List<int>> GenerateAllPermutations()
        {
            List<List<int>> ret = new List<List<int>>();
            bool fl = true;
            bool[] visited = new bool[problem.Clients.Length];

            Generate(0, true, new List<int>(), ret, visited, fl);

            return ret;
        }
    }
}
