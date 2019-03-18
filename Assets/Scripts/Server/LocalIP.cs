#if NETFX_CORE
    using Windows.Networking.Connectivity;
    using System.Linq;
#else
    using System.Net;
    using System.Net.Sockets;
#endif
using UnityEngine;

namespace Server
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalIP
    {
        /// <summary>
        /// Gets the local IP address of the device.
        /// ISSUE: THIS WILL FAIL AND CRASH THE PROGRAM IF THE DEVICE HAS MORE THAN ONE IP ADDRESS.
        /// </summary>
        /// <returns></returns>
        public string Address()
        {
            string localIP = "";
#if NETFX_CORE
            var icp = NetworkInformation.GetInternetConnectionProfile();

            // TODO: Add multiple IP address handling for this.
            if (icp?.NetworkAdapter == null)
                return null;

            var hostNames = NetworkInformation.GetHostNames();

            Debug.Log("IP Hostnames:" + hostNames);

            Windows.Networking.HostName hostName;

            foreach (var host in hostNames)
            {
                Debug.Log("Available hosts:" + host);
            }

            foreach (Windows.Networking.HostName localHostName in NetworkInformation.GetHostNames())
            {
                if (localHostName.IPInformation != null)
                {
                    if (localHostName.Type == Windows.Networking.HostNameType.Ipv4)
                    {
                        localIP = localHostName.ToString();
                        break;
                    }
                }
            }
            
            localIP = localIP == null ? localIP = "No IP found" : localIP;

            Debug.Log("Chosen IP:" + localIP);
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

        /// <summary>
        /// Gets the port for the Blueprint socket.
        /// </summary>
        /// <returns></returns>
        public int Port()
        {
            int port = 9050;
            return port;
        }
    }
}
