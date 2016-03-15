using Autofac;
using Autofac.Core;
using ComputationalCluster.Common;
using System;

namespace ComputationalCluster.Node
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<CommonModule>();
            builder.RegisterType<ComputationalNode>()
                   .AsSelf()
                   .SingleInstance();

            var container = builder.Build();

            try
            {
                var node = container.Resolve<ComputationalNode>();
                node.Start();
            }
            catch (DependencyResolutionException e)
            {
                Console.Error.WriteLine(e.InnerException.Message);
            }
            Console.ReadLine();
        }
    }
}
