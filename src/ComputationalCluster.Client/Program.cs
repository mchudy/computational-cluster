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
            CommonParameterParser.LoadCommandLineParameters(args);

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
    }
}
