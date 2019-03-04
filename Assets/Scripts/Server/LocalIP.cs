/*
Retrieves the IP address and port to communicate on
*/

#if NETFX_CORE
    using Windows.Networking.Sockets;
    using Windows.Networking.Connectivity;
    using System.Linq;
#else
    using UnityEngine;
    using UnityEngine.Networking;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
#endif
using Utils;

namespace Server
{
    public class LocalIP
    {
        // Get local IP address of device.
        public string Address()
        {
            string localIP = "";
#if NETFX_CORE
                var icp = NetworkInformation.GetInternetConnectionProfile();

                if (icp?.NetworkAdapter == null) return null;
                var hostname =
                    NetworkInformation.GetHostNames()
                        .SingleOrDefault(
                            hn =>
                                hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId
                                == icp.NetworkAdapter.NetworkAdapterId);

                // the ip address
                localIP = hostname?.CanonicalName;
#else
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
#endif
            return localIP;
        }

        public int Port()
        {
            int port = 9050;
            return port;
        }
    }
}
