using Autofac;
using Autofac.Core;
using ComputationalCluster.Common;
using System;
namespace ComputationalCluster.Client
{
    class Program
    {
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
                Console.Error.WriteLine(e.InnerException.Message);
            }
            Console.ReadLine();
        }
    }
}
