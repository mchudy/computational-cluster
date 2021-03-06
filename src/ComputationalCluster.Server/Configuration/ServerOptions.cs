﻿using CommandLine;
using CommandLine.Text;

namespace ComputationalCluster.Server.Configuration
{
    public class ServerOptions
    {
        [Option("port", Required = false,
            HelpText = "Listening port number for Communication Server")]
        public int ListeningPort { get; set; }

        [Option("maddress", Required = false,
            HelpText = "Address of Master Server ")]
        public string MasterServerAddress { get; set; }

        [Option("mport", Required = false,
            HelpText = "Listening port of Master Server ")]
        public int MasterServerPort { get; set; }

        [Option('t', "time", Required = false,
            HelpText = "Timeout")]
        public uint Timeout { get; set; }

        [Option("backup", Required = false, DefaultValue = false,
            HelpText = "Backup or Master Server")]
        public bool Backup { get; set; }

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
