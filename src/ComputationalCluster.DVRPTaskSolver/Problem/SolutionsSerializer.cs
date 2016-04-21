﻿using Newtonsoft.Json;
using System.Text;

namespace ComputationalCluster.DVRPTaskSolver.Problem
{
    public class SolutionsSerializer
    {
        public DVRPSolution[] Deserialize(byte[][] solutionsData)
        {
            var solutions = new DVRPSolution[solutionsData.Length];
            for (int i = 0; i < solutionsData.Length; i++)
            {
                var json = Encoding.UTF8.GetString(solutionsData[i]);
                solutions[i] = JsonConvert.DeserializeObject<DVRPSolution>(json);
            }
            return solutions;
        }


        public byte[] Serialize(DVRPSolution solution)
        {
            string json = JsonConvert.SerializeObject(solution);
            return Encoding.UTF8.GetBytes(json);
        }

        public byte[] SerializeForClient(DVRPSolution finalSolution)
        {
            //TODO: better format?
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < finalSolution.Routes.Length; i++)
            {
                foreach (var stop in finalSolution.Routes[i])
                {
                    builder.Append(stop + ",");
                }
                builder.AppendLine();
            }
            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}
