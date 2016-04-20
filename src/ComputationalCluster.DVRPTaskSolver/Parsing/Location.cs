﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.DVRPTaskSolver.Parsing
{
    public class Location
    {

        public int X { get; set; }
        public int Y { get; set; }

        public Location()
        {

        }

        public Location(int X, int Y)
        {
            this.X = X;
            this.Y = Y; 
        }
    }

}