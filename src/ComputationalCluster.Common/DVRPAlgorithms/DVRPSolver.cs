using ComputationalCluster.DVRPTaskSolver.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Common.DVRPAlgorithms
{
    public class DVRPSolver
    {

        DVRPProblemInstance problem;
        double minCost;
        List<Partition> partitions;

        public DVRPSolver(DVRPProblemInstance problem)
        {
            this.problem = problem;
            minCost = double.MaxValue;
        }

        private double TravelDistance(Location l1, Location l2)
        {
            return Math.Sqrt((l1.X - l2.X) * (l1.X - l2.X) + (l1.Y - l2.Y) * (l1.Y - l2.Y));
        }

        double CheckTimesCapacityAndCost(List<int> route)
        {
            for(int i =0; i<route.Count; i++)
            {

            }
            return 0;
        }


        void generuj(int k, bool zDepot, List<int> permutacja, List<List<int>> ret, bool[] tab, bool flag)
        {
            if (k == problem.Clients.Length)
            {
                permutacja.Add(0);
                if (flag)
                {
                    ret.Add(permutacja);
                    flag = false;
                }
                else
                    flag = true;
                return;
            }

            for (int m = 1; m < problem.Clients.Length + 1; ++m)
            {
                var p = new List<int>(permutacja);
                if (!tab[m - 1])
                {
                    tab[m - 1] = true;

                    if (zDepot || k ==0)
                        p.Add(0);

                    p.Add(m);
                    generuj(k + 1, true, new List<int>(p), ret , tab, flag);
                    generuj(k + 1, false, new List<int>(p), ret, tab, flag);

                    tab[m - 1] = false;
                }
            }

        }

        List<List<int>> GenerateAllPermutations()
        {
            List<List<int>> ret = new List<List<int>>();
            bool fl = true;
            bool[] visited = new bool[problem.Clients.Length];

            generuj(0, true, new List<int>(),  ret, visited,  fl);

            return ret;
        }

    }
}
