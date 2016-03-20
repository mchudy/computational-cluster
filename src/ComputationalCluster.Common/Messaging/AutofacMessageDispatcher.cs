using Autofac;
using ComputationalCluster.Common.Messages;
using ComputationalCluster.Common.Networking;
using System.Reflection;

namespace ComputationalCluster.Common.Messaging
{
    public class AutofacMessageDispatcher : IMessageDispatcher
    {
        private const string methodName = nameof(IMessageHandler<RegisterMessage>.HandleMessage);

        private readonly IComponentContext context;

        public AutofacMessageDispatcher(IComponentContext context)
        {
            this.context = context;
        }

        public void Dispatch<T>(T message, ITcpClient client) where T : Message
        {
            var type = typeof(IMessageHandler<>).MakeGenericType(message.GetType());
            var handler = context.Resolve(type);
            type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, handler,
                new object[] { message, client });
        }
    }
}
