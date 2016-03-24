using log4net;
using System.Configuration;

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
            AcceptSingleDashes(parameters);
            bool parse = CommandLine.Parser.Default.ParseArguments(parameters, options);

            if (parse)
            {
                if (options.ServerAddress != null && ConfigurationManager.AppSettings["ServerAddress"] != null)
                {
                    ConfigurationManager.AppSettings["ServerAddress"] = options.ServerAddress;
                }
                if (ConfigurationManager.AppSettings["ServerPort"] != null
                        && options.ServerPort.ToString() != ConfigurationManager.AppSettings["ServerPort"])
                {
                    ConfigurationManager.AppSettings["ServerPort"] = options.ServerPort.ToString();
                }

                ConfigurationManager.RefreshSection("AppSettings");

                return true;
            }
            return false;
        }

        public static void AcceptSingleDashes(string[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                var param = parameters[i];
                if (param.StartsWith("-") && param[1] != '-')
                {
                    parameters[i] = "-" + param;
                }
            }
        }
    }
}
