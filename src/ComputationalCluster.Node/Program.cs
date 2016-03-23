using Autofac;
using Autofac.Core;
using ComputationalCluster.Common;
using ComputationalCluster.Common.Messaging;
using log4net;
using System;
using System.Reflection;

namespace ComputationalCluster.Node
{
    class Program
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(typeof(Constants).Assembly);
            builder.RegisterType<ComputationalNode>()
                   .AsSelf()
                   .SingleInstance();
            builder.RegisterType<NodeContext>()
                   .AsSelf()
                   .SingleInstance();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                   .AsClosedTypesOf(typeof(IResponseHandler<>))
                   .InstancePerDependency();

            var container = builder.Build();

            CommonParameterParser.LoadCommandLineParameters(args);
            try
            {
                var node = container.Resolve<ComputationalNode>();
                node.Start();
            }
            catch (DependencyResolutionException e)
            {
                logger.Error(e.InnerException.Message);
            }
            Console.ReadLine();
        }
    }
}
