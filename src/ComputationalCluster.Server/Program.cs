using Autofac;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Common.Serialization;
using ComputationalCluster.Server.Configuration;
using System;
using System.Configuration;
using log4net;

namespace ComputationalCluster.Server
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            if (!LoadCommandLineParameters(args)) return;
            var container = BuildContainer();
            var server = container.Resolve<Server>();
            server.Start();
        }

        private static bool LoadCommandLineParameters(string[] args)
        {
            var options = new ServerOptions();

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                System.Configuration.Configuration config =
                    ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings["ListeningPort"].Value = options.ListeningPort.ToString();
                config.AppSettings.Settings["Timeout"].Value = options.Timeout.ToString();
                if (options.Backup)
                {
                    config.AppSettings.Settings["Mode"].Value = ServerMode.Backup.ToString();
                    config.AppSettings.Settings.Add("MasterServerAddress",options.MasterServerAddress.ToString());
                    config.AppSettings.Settings.Add("MasterServerPort",options.MasterServerPort.ToString());
                }
                else
                {
                    config.AppSettings.Settings["Mode"].Value = ServerMode.Primary.ToString();
                }

                config.Save(ConfigurationSaveMode.Modified);

                ConfigurationManager.RefreshSection("appSettings");

                logger.Info($"Port: {options.ListeningPort}");
                logger.Info($"Timeout: {options.Timeout}");
                logger.Info($"Backup {options.Backup}");
                logger.Info($"MAddres: {options.MasterServerAddress}");
                logger.Info($"MPort: {options.MasterServerPort}");

                return true;
            }
            return false;
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MessageSerializer>()
                   .As<IMessageSerializer>();
            builder.RegisterType<ServerMessenger>()
                   .As<IServerMessenger>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<ServerContext>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .AsClosedTypesOf(typeof(IMessageHandler<>));
            builder.RegisterType<AutofacMessageDispatcher>()
                   .AsImplementedInterfaces();
            builder.RegisterType<Server>().AsSelf();
            builder.RegisterType<ServerConfiguration>()
                   .As<IServerConfiguration>()
                   .SingleInstance();
            builder.RegisterType<StatusChecker>()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            return builder.Build();
        }
    }
}
