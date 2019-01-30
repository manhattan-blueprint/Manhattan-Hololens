#if NETFX_CORE
    using Windows.Networking;
    using Windows;
#else
    // Your standard code here
    using System;
    using System.Net;
    using System.Net.Sockets;
#endif

using System.Text;
using UnityEngine;

public class SynchronousSocketListener
{
    private ServerState serverState;
    private LocalIP localIP;
    private string localIPAddress;
    private int port;

    // Incoming data from the client.  
    public static string data = null;

    public SynchronousSocketListener(LocalIP localIP, ServerState serverState) {
        this.serverState = serverState;
        this.localIP = localIP;
        localIPAddress = localIP.Address();
        port = localIP.Port();
    }

    public void StartListening() {
        // Data buffer for incoming data.  
        byte[] bytes = new Byte[1024];

        // Establish the local endpoint for the socket.  
        // Dns.GetHostName returns the name of the   
        // host running the application.  
        IPAddress ipAddress = IPAddress.Parse(localIPAddress);
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);

        // Create a TCP/IP socket.  
        Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        // Bind the socket to the local endpoint and   
        // listen for incoming connections.
        try {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            // Start listening for connections.  
            while (true) {
                Debug.Log("Server: Waiting for a connection");

                // Program is suspended while waiting for an incoming connection.  
                Socket handler = listener.Accept();
                data = null;

                // serverState uses the most recently connected device as the
                // device in use

                // Message will be less that minimal length; no need to accomodate
                // "<EOF>".
                int bytesRec = handler.Receive(bytes);

                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                Debug.Log("Server: Received " + bytesRec + " bytes, of form " + data);

                if (bytesRec > 0) {
                    serverState.AddInstruction(data);
                }

                // Echo the data back to the client.  
                byte[] msg = Encoding.ASCII.GetBytes(data);

                Debug.Log("Server: Sending " + bytesRec + " bytes, of form " + msg);
   
            }
        }

        catch (Exception e) {
            Debug.Log(e.ToString());
        }

        Debug.Log("Synchronous Sever exited a while (True) loop without a break :o");
    }
}