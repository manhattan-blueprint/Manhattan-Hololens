//Attach this script to a GameObject for debugging.
//Create a Text GameObject (Create>UI>Text) and attach it to the My Text field in the Inspector of your GameObject
//Press the space bar in Play Mode to see the Text change.

using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class LocalIP {
    // Get local IP address of device.
    public string Address() {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
