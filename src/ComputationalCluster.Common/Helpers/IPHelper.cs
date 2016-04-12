using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace ComputationalCluster.Common.Helpers
{
    public static class IPHelper
    {
        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            var localIp = host.AddressList
                .First(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            return localIp;
        }

        public static bool AreEqual(string address1, string address2)
        {
            IPAddress ip1 = IPAddress.Parse(address1);
            IPAddress ip2 = IPAddress.Parse(address2);
            return ip1.Equals(ip2);
        }
    }
}
