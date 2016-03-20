using Autofac;
using Autofac.Core;
using ComputationalCluster.Common;
using log4net;
using System;

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
