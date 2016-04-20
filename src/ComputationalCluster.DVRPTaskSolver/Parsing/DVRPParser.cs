using System;
using System.IO;
using System.Text;

namespace ComputationalCluster.DVRPTaskSolver.Parsing
{
    public class DVRPParser : IDVRPParser
    {
        public DVRPProblemInstance ParseFile(byte[] data)
        {
            string text = Encoding.UTF8.GetString(data);
            var reader = new StreamReader(new MemoryStream(data), Encoding.UTF8);
            return Parse(reader);
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

            int[] depotIndexes;
            int[] clientIndexes;
            Location[] locations;

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
                        numCapacities = int.Parse(items[1]);
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
            }

            problem.Clients = new Client[numVisits];
            problem.Depots = new Depot[numDepots];
            problem.VehicleCapacity = capacities;
            problem.VehiclesCount = numVehicles;

            depotIndexes = new int[numDepots];
            clientIndexes = new int[numVisits];
            locations = new Location[numDepots + numVisits];

            InitializeClients(problem.Clients);
            InitializeDepots(problem.Depots);

            while ((line = reader.ReadLine()) != "EOF")
            {
                string[] items = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                switch (items[0])
                {
                    case "DEPOTS":
                        ProccessDepots(reader,depotIndexes);
                        break;
                    case "DEMAND_SECTION":
                        ProccessDemands(reader, problem.Clients,numDepots);
                        break;
                    case "LOCATION_COORD_SECTION":
                        ProccessLocations(reader, locations);
                        break;
                    case "DEPOT_LOCATION_SECTION":
                        ProccesDepotLocations(reader,problem.Depots,locations);
                        break;
                    case "VISIT_LOCATION_SECTION":
                        ProccesClientsLocations(reader, problem.Clients,locations,numDepots);
                        break;
                    case "DURATION_SECTION":
                        ProccesDurations(reader, problem.Clients, numDepots);
                        break;
                    case "DEPOT_TIME_WINDOW_SECTION":
                        ProccesDepotTimeWndowSection(reader, problem.Depots);
                        break;
                    case "TIME_AVAIL_SECTION":
                        ProccesTimeAvail(reader, problem.Clients, numDepots);
                        break;
                    default:
                        break;
                }
            }
            return problem;
        }

        private void InitializeDepots(Depot[] depots)
        {
            for (int i = 0; i < depots.Length; i++)
            {
                depots[i] = new Depot();
            }
        }

        private void InitializeClients(Client[] clients)
        {
            for (int i = 0; i < clients.Length; i++)
            {
                clients[i] = new Client();
            }
        }

        private void ProccesTimeAvail(StreamReader reader, Client[] clients, int numDepots)
        {
            for (int i = 0; i < clients.Length; i++)
            {
                string[] items = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int clientIndex = int.Parse(items[0]);
                int timeAvail = int.Parse(items[1]);
                clients[clientIndex - numDepots].AvailableTime = timeAvail;
            }
        }

        private void ProccesDepotTimeWndowSection(StreamReader reader, Depot[] depots)
        {
            for (int i = 0; i < depots.Length; i++)
            {
                string[] items = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int depotIndex = int.Parse(items[0]);
                int startHour = int.Parse(items[1]);
                int endHour = int.Parse(items[2]);
                depots[depotIndex].StartHour = startHour;
                depots[depotIndex].EndHour = endHour;
            }
        }

        private void ProccesDurations(StreamReader reader, Client[] clients, int numDepots)
        {
            for (int i = 0; i < clients.Length; i++)
            {
                string[] items = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int clientIndex = int.Parse(items[0]);
                int duration = int.Parse(items[1]);
                clients[clientIndex - numDepots].UnloadTime = duration;
            }
        }

        private void ProccesClientsLocations(StreamReader reader, Client[] clients, Location[] locations, int numDepots)
        {
            for (int i = 0; i < clients.Length; i++)
            {
                string[] items = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int clientIndex = int.Parse(items[0]);
                int locationIndex = int.Parse(items[1]);
                clients[clientIndex - numDepots].X = locations[locationIndex].X;
                clients[clientIndex - numDepots].Y = locations[locationIndex].Y;
            }
        }

        private void ProccesDepotLocations(StreamReader reader, Depot[] depots, Location[] locations)
        {
            for (int i = 0; i < depots.Length; i++)
            {
                string[] items = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int depotIndex = int.Parse(items[0]);
                int locationIndex = int.Parse(items[1]);
                depots[depotIndex].X = locations[locationIndex].X;
                depots[depotIndex].Y = locations[locationIndex].Y;
            }
        }

        private void ProccessLocations(StreamReader reader, Location[] locations)
        {
            for (int i = 0; i < locations.Length; i++)
            {
                string[] items = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int locationIndex = int.Parse(items[0]);
                locations[locationIndex] = new Location(int.Parse(items[1]), int.Parse(items[2]));
            }
        }

        private void ProccessDemands(StreamReader reader, Client[] clients, int numDepots)
        {
            for (int i = 0; i < clients.Length; i++)
            {
                string[] items = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int clientindex = int.Parse(items[0]);
                clients[clientindex - numDepots].DemandSize = Math.Abs(int.Parse(items[1]));
            }
        }

        private void ProccessDepots(StreamReader reader, int[] depotIndexes)
        {
            for (int i = 0; i < depotIndexes.Length; i++)
            {
               string index = reader.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                depotIndexes[i] = int.Parse(index); 
            }
        }
    }
}
