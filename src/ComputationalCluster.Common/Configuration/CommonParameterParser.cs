using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalCluster.Common
{
    public static class CommonParameterParser
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(CommonParameterParser));

        public static void LoadCommandLineParameters()
        {
            var options = new CommonOptions();
            logger.Info($"Set Parameters: ");
            var parameters = Console.ReadLine();
            while (!ParseParameters(parameters, ref options))
            {
                logger.Info($"Set Parameters: ");
                Console.ReadLine();
            }
        }

        public static bool ParseParameters(string parameters, ref CommonOptions options)
        {
            //Options will be taken form App.Settings
            if (string.IsNullOrWhiteSpace(parameters))
            {
                logger.Info($"Setting default parameters");
                return true;
            }

            bool parse = CommandLine.Parser.Default.ParseArguments(parameters.Split(' '), options);
            if (parse)
            {
                System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (options.ServerAddress != null)
                {
                    config.AppSettings.Settings["ServerAddress"].Value = options.ServerAddress.ToString();
                    logger.Info($"Server Address: {options.ServerAddress}");
                }
                if (options.ServerPort.ToString() != ConfigurationManager.AppSettings["ServerPort"])
                {
                    config.AppSettings.Settings["ServerPort"].Value = options.ServerPort.ToString();
                    logger.Info($"Server Port: {options.ServerPort}");
                }
                  
                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");

                return true;
            }
            return false;
        }

    }
}
