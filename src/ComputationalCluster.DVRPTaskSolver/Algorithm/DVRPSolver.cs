using ComputationalCluster.DVRPTaskSolver.Problem;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ComputationalCluster.DVRPTaskSolver.Algorithm
{
    public class DVRPSolver
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(DVRPSolver));
        private DVRPPartialProblem partialProblem;
        private DVRPProblemInstance problem;
        double minCost;
        List<int>[] currentBestSolution;
        List<Partition> partitions;
        List<Location> locations;

        public DVRPSolver(DVRPPartialProblem partialProblem)
        {
            this.partialProblem = partialProblem;
            this.problem = partialProblem.ProblemInstance;
            minCost = double.MaxValue;
            this.partitions = partialProblem.Partitions;

            this.locations = new List<Location>();
            this.locations.Add(problem.Depots[0]);

            for (int i = 0; i < problem.Clients.Length; i++)
                this.locations.Add(problem.Clients[i]);

            currentBestSolution = new List<int>[problem.VehiclesCount];
        }

        public DVRPSolution Solve()
        {
            DVRPSolution solution = new DVRPSolution();
            Stopwatch stopwatch = Stopwatch.StartNew();

            var allPartitions = partitions.Count;
            logger.Info($"[Task Solver] Checking {allPartitions} partitions");

            int current = 0;
            foreach (var partition in partitions)
            {
                double currCost = 0;
                List<int>[] currentSolution = new List<int>[problem.VehiclesCount];
                for (int i = 0; i < partition.truckClients.Length; i++)
                {
                    if (partition.truckClients[i].Count == 0)
                        continue;

                    double bestCost = double.MaxValue;
                    List<int> bestRoute = null;
                    foreach (var perm in GenerateAllPermutations(partition.truckClients[i].Count))
                    {
                        for (int j = 0; j < perm.Count; j++)
                            if (perm[j] != 0)
                                perm[j] = partition.truckClients[i][perm[j] - 1];

                        double c = CheckCapacitiesTimeAndCost(perm);
                        if (c <= bestCost)
                        {
                            bestCost = c;
                            bestRoute = perm;
                        }
                    }
                    currCost += bestCost;
                    currentSolution[i] = bestRoute;
                }
                if (currCost <= minCost)
                {
                    minCost = currCost;
                    solution.Cost = minCost;
                    solution.Routes = currentSolution;
                }
                current++;
                double progress = ((double)current / allPartitions);
                Console.Write("\r{0:P2}", progress);
            }

            for (int i = 0; i < solution.Routes.Length; i++)
            {
                if (solution.Routes[i] == null)
                    solution.Routes[i] = new List<int>();
            }

            stopwatch.Stop();
#if DEBUG
            Console.WriteLine();
#endif
            logger.Info($"[Task Solver] Computation time: {stopwatch.Elapsed}");
            return solution;
        }

        private static double TravelDistance(Location l1, Location l2)
        {
            return Math.Sqrt((l1.X - l2.X) * (l1.X - l2.X) + (l1.Y - l2.Y) * (l1.Y - l2.Y));
        }

        double CheckCapacitiesTimeAndCost(List<int> route)
        {
            double currCost = 0;
            double currTime = problem.Depots[0].StartTime;
            int towar = problem.VehicleCapacity;
            double trlDist;
            for (int i = 1; i < route.Count; i++)
            {
                trlDist = TravelDistance(locations[route[i]], locations[route[i - 1]]);
                if (route[i] == 0)
                {
                    towar = problem.VehicleCapacity;
                    currCost += trlDist;
                    currTime += trlDist;
                }
                else
                {
                    if (towar < ((Client)locations[route[i]]).DemandSize)   //checking if there's enough cargo in the truck
                        return double.MaxValue;

                    towar -= ((Client)locations[route[i]]).DemandSize;  //updating cargo

                    if (currTime + trlDist > problem.Clients[route[i] - 1].AvailableTime) //checking if we arrive before an order becomes available
                        currTime += trlDist;
                    else
                        currTime = problem.Clients[route[i] - 1].AvailableTime;
                    currCost += trlDist;
                    currTime += problem.Clients[route[i] - 1].UnloadTime;   //update on time with time required to unload
                }
            }

            // if (currTime > problem.Depots[0].EndTime )    //checking if we return to a depo before it closes
            //       return double.MaxValue;

            return currCost;
        }


        private static IEnumerable<List<int>> Generate(int k, bool zDepot, List<int> permutation, bool[] tab, int number)
        {
            if (k == number)
            {
                if (zDepot)
                {
                    permutation.Add(0);
                    yield return permutation;
                }
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
                    foreach (var pe in Generate(k + 1, true, new List<int>(p), tab, number))
                    {
                        yield return pe;
                    }
                    foreach (var pe in Generate(k + 1, false, new List<int>(p), tab, number))
                    {
                        yield return pe;
                    }

                    tab[m - 1] = false;
                }
            }

        }

        private IEnumerable<List<int>> GenerateAllPermutations(int numbers)
        {
            bool[] visited = new bool[problem.Clients.Length];
            return Generate(0, true, new List<int>(), visited, numbers);
        }
    }
}
