//Attach this script to a GameObject for debugging.
//Create a Text GameObject (Create>UI>Text) and attach it to the My Text field in the Inspector of your GameObject
//Press the space bar in Play Mode to see the Text change.

using UnityEngine;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Net;
using System.Net.Sockets;


public class UDPclient : MonoBehaviour {

	ConnectionConfig config;
	HostTopology topology;

	AsynchronousSocketListener listener = new AsynchronousSocketListener();

    private void Start() {
				listener.StartListening(8888);
    }

    //This is the function that serializes the message before sending it
    void sendMessage(string textInput)
    {

    }

    private void Update() {
        //     int outHostId;
        //     int outConnectionId;
        //     int outChannelId;
        //     byte[] buffer = new byte[1024];
        //     int bufferSize = 1024;
        //     int receiveSize;
        //     byte error;
				//
        //     NetworkEventType evnt = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, bufferSize, out receiveSize, out error);
        //     switch (evnt)
        //     {
        //         case NetworkEventType.ConnectEvent:
        //             if (outHostId == hostId &&
        //                 outConnectionId == connectionId &&
        //                 (NetworkError)error == NetworkError.Ok)
        //             {
        //                 Debug.Log("Connected");
        //             }
        //             break;
        //         case NetworkEventType.DisconnectEvent:
        //             if (outHostId == hostId &&
        //                 outConnectionId == connectionId)
        //             {
        //                 Debug.Log("Connected, error:" + error.ToString());
        //             }
        //             break;
        //     }
        // }
				//
        // // Press the space key to send a test message.
        // if (Input.GetKey(KeyCode.Space))
        // {
        //     sendMessage("KILL ALL HUMANS.");
        // }
				//
        // // Press the F1 key to get status on connection.
        // if (Input.GetKey(KeyCode.F1))
        // {
        //     Debug.Log("Current error state:" + error);
        // }
    }

    // Get local IP address of device.
    public string LocalIPAddress()
    {
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
