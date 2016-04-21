using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputationalCluster.DVRPTaskSolver.Problem;

namespace ComputationalCluster.DVRPTaskSolver.Parsing
{
    public interface IDVRPParser
    {
        DVRPProblemInstance Parse(byte[] data);   
    }
}
