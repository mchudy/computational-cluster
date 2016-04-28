using Autofac;
using Autofac.Core;
using ComputationalCluster.Common;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;
using System.Linq;

namespace ComputationalCluster.Client
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            // skipping file path
            string[] argsToParse = args.Take(args.Length - 1).ToArray();
            CommonParameterParser.LoadCommandLineParameters(argsToParse);

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
                client.Start(args.LastOrDefault());
                Console.ReadLine();
            }
            catch (DependencyResolutionException e)
            {
                logger.Error(e.InnerException.Message);
            }
            catch (ArgumentException e)
            {
                logger.Error(e.Message);
            }
        }
    }
}
