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
        List<int>[] currentBestSolution;
        List<Partition> partitions;
        List<Location> locations;

        public DVRPSolver(DVRPPartialProblem partialProblem )
        {
            this.partialProblem = partialProblem;
            this.problem = partialProblem.ProblemInstance;
            minCost = double.MaxValue;
            this.partitions = partialProblem.Partitions;

            this.locations = new List<Location>();
            this.locations.Add(problem.Depots[0]);

            for(int i=0; i<problem.Clients.Length; i++)
                this.locations.Add(problem.Clients[i]);

            currentBestSolution = new List<int>[problem.VehiclesCount];
        }

        public DVRPSolution Solve()
        {
            DVRPSolution solution = new DVRPSolution();

            foreach(var partition in partitions)
            {
                double currCost = 0;
                List<int>[] currentSolution = new List<int>[problem.VehiclesCount];
                for(int i =0; i<partition.truckClients.Length; i++)
                {
                    if (partition.truckClients[i].Count == 0)
                        continue;

                    List<List<int>> permutations = GenerateAllPermutations(partition.truckClients[i].Count);

                    foreach (var perm in permutations)
                        for (int j = 0; j < perm.Count; j++)
                            if (perm[j] != 0)
                                perm[j] = partition.truckClients[i][perm[j]];

                    double bestCost = double.MaxValue;
                    List<int> bestRoute = null;

                    foreach(var route in permutations)
                    {
                        double c = CheckCapacitiesAndCost(route);
                        if(c<bestCost)
                        {
                            bestCost = c;
                            bestRoute = route;
                        }
                    }

                    currCost += bestCost;
                    currentSolution[i] = bestRoute;
                }
                if(currCost < minCost)
                {
                    minCost = currCost;
                    solution.Cost = minCost;
                    solution.Routes = currentSolution;
                }
            }

            return solution;
        }

        private double TravelDistance(Location l1, Location l2)
        {
            return Math.Sqrt((l1.X - l2.X) * (l1.X - l2.X) + (l1.Y - l2.Y) * (l1.Y - l2.Y));
        }

        double CheckCapacitiesAndCost(List<int> route)
        {
            double currCost = 0;
            int towar = problem.VehicleCapacity;
            for (int i = 1; i < route.Count; i++)
            {

                currCost += TravelDistance(locations[i], locations[i - 1]);

                if (route[i] == 0)
                    towar = problem.VehicleCapacity;
                else
                {
                    if (towar < ((Client)locations[i]).DemandSize)
                        return double.MaxValue;

                    towar -= ((Client)locations[i]).DemandSize;
                }
            }
            return currCost;
        }


        void Generate(int k, bool zDepot, List<int> permutation, List<List<int>> ret, bool[] tab, bool flag, int number)
        {
            if (k == number)
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

            for (int m = 1; m < number + 1; ++m)
            {
                var p = new List<int>(permutation);
                if (!tab[m - 1])
                {
                    tab[m - 1] = true;

                    if (zDepot || k == 0)
                        p.Add(0);

                    p.Add(m);
                    Generate(k + 1, true, new List<int>(p), ret, tab, flag, number);
                    Generate(k + 1, false, new List<int>(p), ret, tab, flag, number);

                    tab[m - 1] = false;
                }
            }

        }

        List<List<int>> GenerateAllPermutations(int numbers)
        {
            List<List<int>> ret = new List<List<int>>();
            bool fl = true;
            bool[] visited = new bool[problem.Clients.Length];

            Generate(0, true, new List<int>(), ret, visited, fl, numbers);

            return ret;
        }
    }
}
