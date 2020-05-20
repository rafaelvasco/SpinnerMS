using System.Linq;
using Microsoft.AspNetCore.Http;

namespace SpinnerMS.Utils
{
    public static class RequestUtils
    {
        public static (string Ip, string Port) GetRemoteIpAndPort(HttpRequest request)
        {
            var remote_ip_address = request.HttpContext.Connection.RemoteIpAddress;
            var remote_port = request.HttpContext.Connection.RemotePort;

            if (remote_ip_address != null)
            {
                if (remote_ip_address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    remote_ip_address = System.Net.Dns.GetHostEntry(remote_ip_address).AddressList
                        .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
                return (remote_ip_address.ToString(), remote_port.ToString());
            }

            return (string.Empty, string.Empty);
        }
    }
}
