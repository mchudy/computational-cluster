using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.DVRPTaskSolver.Parsing
{
    public interface IDVRPParser
    {
        DVRPProblemInstance ParseFile(string path);   
    }
}
