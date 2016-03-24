using Autofac;
using ComputationalCluster.Common;
using ComputationalCluster.Common.Messaging;
using ComputationalCluster.Server.Configuration;
using log4net;
using System.Configuration;

namespace ComputationalCluster.Server
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            LoadCommandLineParameters(args);
            var container = BuildContainer();
            var server = container.Resolve<Server>();
            server.Start();
        }

        private static void LoadCommandLineParameters(string[] args)
        {
            var options = new ServerOptions();
            ParseParameters(args, ref options);

        }

        private static bool ParseParameters(string[] parameters, ref ServerOptions options)
        {
            //Options will be taken form App.Settings
            if (parameters.Length == 0)
            {
                logger.Info($"Setting default parameters");
                return true;
            }
            CommonParameterParser.AcceptSingleDashes(parameters);
            bool parse = CommandLine.Parser.Default.ParseArguments(parameters, options);
            if (parse)
            {

                ConfigurationManager.AppSettings["ListeningPort"] = options.ListeningPort.ToString();
                ConfigurationManager.AppSettings["Timeout"] = options.Timeout.ToString();
                if (options.Backup)
                {
                    ConfigurationManager.AppSettings["Mode"] = ServerMode.Backup.ToString();
                    ConfigurationManager.AppSettings["MasterServerAddress"] = options.MasterServerAddress;
                    ConfigurationManager.AppSettings["MasterServerPort"] = options.MasterServerPort.ToString();
                    logger.Info($"Backup {options.Backup}");
                    logger.Info($"MAddres: {options.MasterServerAddress}");
                    logger.Info($"MPort: {options.MasterServerPort}");
                }
                else
                {
                    ConfigurationManager.AppSettings["Mode"] = ServerMode.Primary.ToString();
                }

                ConfigurationManager.RefreshSection("AppSettings");

                logger.Info($"Port: {options.ListeningPort}");
                logger.Info($"Timeout: {options.Timeout}");
                return true;
            }
            return false;
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(typeof(Constants).Assembly);
            builder.RegisterType<ServerMessenger>()
                   .As<IServerMessenger>()
                   .InstancePerDependency();
            builder.RegisterType<ServerContext>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .AsClosedTypesOf(typeof(IMessageHandler<>))
                   .InstancePerDependency();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .AsClosedTypesOf(typeof(IResponseHandler<>))
                   .InstancePerDependency();
            builder.RegisterType<AutofacMessageDispatcher>()
                   .AsImplementedInterfaces();
            builder.RegisterType<Server>()
                   .AsSelf()
                   .SingleInstance();
            builder.RegisterType<ServerConfiguration>()
                   .AsImplementedInterfaces()
                   .SingleInstance();
            builder.RegisterType<StatusChecker>()
                   .AsImplementedInterfaces()
                   .SingleInstance();

            return builder.Build();
        }
    }
}
