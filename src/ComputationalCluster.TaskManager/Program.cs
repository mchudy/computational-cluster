using Autofac;
using Autofac.Core;
using ComputationalCluster.Common;
using System;

namespace ComputationalCluster.TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(Constants).Assembly);
            builder.RegisterType<TaskManager>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<TaskManagerContext>().AsSelf().SingleInstance();
            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                   .AsClosedTypesOf(typeof(IResponseHandler<>));

            var container = builder.Build();

            CommonParameterParser.LoadCommandLineParameters();

            try
            {
                var taskManager = container.Resolve<TaskManager>();
                taskManager.Start();
            }
            catch (DependencyResolutionException e)
            {
                Console.Error.WriteLine(e.InnerException.Message);
            }
            Console.ReadLine();
        }
    }
}
