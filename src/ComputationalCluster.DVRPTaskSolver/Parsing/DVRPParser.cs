using System;
using System.IO;

namespace ComputationalCluster.DVRPTaskSolver.Parsing
{
    public class DVRPParser : IDVRPParser
    {
        public DVRPProblemInstance ParseFile(string path)
        {
            if(File.Exists(path))
            {
                StreamReader reader = File.OpenText(path);
                return Parse(reader);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        private DVRPProblemInstance Parse(StreamReader reader)
        {
            DVRPProblemInstance problem = new DVRPProblemInstance();
            string line;

            int numDepots = 0;
            int numCapacities = 0;
            int numVisits = 0;
            int numVehicles = 0;
            int capacities = 0;

            //parsing first part
            while (!(line = reader.ReadLine()).StartsWith("DATA_SECTION"))
            {
                string[] items = line.Split(new char[] {' '},StringSplitOptions.RemoveEmptyEntries);
                
                switch (items[0])
                {
                    case "NUM_DEPOTS:":
                        numDepots = int.Parse(items[1]);
                        break;
                    case "NUM_CAPACITIES:":
                        numCapacities = 1;
                        break;
                    case "NUM_VISITS:":
                        numVisits = int.Parse(items[1]);
                        break;
                    case "NUM_VEHICLES:":
                        numVehicles = int.Parse(items[1]);
                        break;
                    case "CAPACITIES:":
                        if (numCapacities > 0) capacities = int.Parse(items[1]);
                        break;
                    default:
                        break;
                }

                problem.Clients = new Client[numVisits];
                problem.Depots = new Depot[numDepots];
                problem.VehicleCapacity = capacities;
                problem.VehiclesCount = numVehicles;

                //TODO
            }
            return problem;
        }   

    }
}
