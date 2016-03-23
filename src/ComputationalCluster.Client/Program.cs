using Autofac;
using Autofac.Core;
using ComputationalCluster.Common;
using log4net;
using System;
using System.Configuration;

namespace ComputationalCluster.Client
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(typeof(Constants).Assembly);
            builder.RegisterType<Client>()
                .AsSelf()
                .SingleInstance();

            LoadCommandLineSettings(args);

            var container = builder.Build();

            try
            {
                var client = container.Resolve<Client>();
                client.Start();
            }
            catch (DependencyResolutionException e)
            {
                logger.Error(e.InnerException.Message);
            }
            Console.ReadLine();
        }

        private static void LoadCommandLineSettings(string[] args)
        {
            ClientOptions options = new ClientOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                System.Configuration.Configuration config =
                   ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["ServerAddress"].Value = options.ServerAddress.ToString();
                config.AppSettings.Settings["ServerPort"].Value = options.ServerPort.ToString();

                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");

                logger.Info($"Server Address: {options.ServerAddress}");
                logger.Info($"Server Port: {options.ServerPort}");
            }
        }
    }
}
