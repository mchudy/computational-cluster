﻿using log4net;
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

        public static void LoadCommandLineParameters(string[] args)
        {
            var options = new CommonOptions();
            ParseParameters(args, ref options);
        }

        public static bool ParseParameters(string[] parameters, ref CommonOptions options)
        {
            //Options will be taken form App.Settings
            if (parameters.Length == 0)
            {
                logger.Info($"Setting default parameters");
                return true;
            }

            bool parse = CommandLine.Parser.Default.ParseArguments(parameters, options);
            if (parse)
            {
                System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if (options.ServerAddress != null && config.AppSettings.Settings["ServerAddress"] != null)
                {
                    config.AppSettings.Settings["ServerAddress"].Value = options.ServerAddress.ToString();
                }
                if (ConfigurationManager.AppSettings["ServerPort"] != null 
                        && options.ServerPort.ToString() != ConfigurationManager.AppSettings["ServerPort"] )
                {
                    config.AppSettings.Settings["ServerPort"].Value = options.ServerPort.ToString();
                }
                  
                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");

                return true;
            }
            return false;
        }

    }
}
