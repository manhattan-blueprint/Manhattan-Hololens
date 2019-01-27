/*
Manages all
Attach this script to a GameObject. Create a Text GameObject (Create>UI>Text)
and attach it to the My Text field in the Inspector of your GameObject. Press
the space bar in Play Mode to see the Text change.
*/

using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Server {
    private String greetMessage;
    private String connectedMessage;
    private String clientAddress;
    private State state;
    Socket s;
    BufferedReader input;
    long delayedTimer;

    private string IPAddress;
	  ConnectionConfig config;
	  HostTopology topology;
    Thread serverThread;
    Thread clientThread;
    LocalIP localIP;
    AsynchronousSocketListener listener;
    private string LocalIPAddress;
    private int Port;

    public void Start() {
        localIP = new LocalIP();
        listener = new AsynchronousSocketListener();
        LocalIPAddress = localIP.Address();
        port = localIP.Port();
        Debug.Log("World initialized with server on IP " + LocalIPAddress + " through port " + Port);

        serverThread = new Thread(new ThreadStart(listener.StartListening));
        serverThread.Start();
    }

    //This is the function that serializes the message before sending it
    void sendMessage(string textInput) {

    }

    private void Update() {
    }
}
