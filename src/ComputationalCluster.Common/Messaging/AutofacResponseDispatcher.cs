using Autofac;
using ComputationalCluster.Common.Messages;
using log4net;
using System.Reflection;

namespace ComputationalCluster.Common.Messaging
{
    public class AutofacResponseDispatcher : IResponseDispatcher
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(AutofacResponseDispatcher));

        private const string methodName = nameof(IResponseHandler<RegisterMessage>.HandleResponse);

        private readonly IComponentContext context;

        public AutofacResponseDispatcher(IComponentContext context)
        {
            this.context = context;
        }

        public void Dispatch<T>(T message) where T : Message
        {
            var type = typeof(IResponseHandler<>).MakeGenericType(message.GetType());
            var handler = context.ResolveOptional(type);
            if (handler == null)
            {
                logger.Fatal($"Could not find handler for {message.GetType().Name}");
            }
            type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, handler,
                new object[] { message });
        }
    }
}
