using Autofac;
using Autofac.Core;
using ComputationalCluster.Common;
using ComputationalCluster.Common.Messaging;
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
            if (!LoadCommandLineSettings(args)) return;

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(typeof(Constants).Assembly);
            builder.RegisterType<Client>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<ClientContext>().AsSelf().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .AsClosedTypesOf(typeof(IResponseHandler<>))
                   .InstancePerDependency();

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

        private static bool LoadCommandLineSettings(string[] args)
        {
            return true;
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
                return true;
            }
            return true; //false
        }
    }
}
