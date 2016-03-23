using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Client
{
    class ClientOptions
    {
        [Option("address", Required = true,
            HelpText = "Server Address")]
        public string ServerAddress { get; set; }

        [Option("port", Required = true,
            HelpText = "Server Port ")]
        public int ServerPort { get; set; }
    }
}
