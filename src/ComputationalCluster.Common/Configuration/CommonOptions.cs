using CommandLine;
using CommandLine.Text;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Common
{
    public class CommonOptions
    {
        [Option("address", Required = false,
            HelpText = "Server Address")]
        public string ServerAddress { get; set; }

        [Option("port", Required = false,
            HelpText = "Server Port ")]
        public int ServerPort { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
