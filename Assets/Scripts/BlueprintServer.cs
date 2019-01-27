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

public class BlueprintServer : MonoBehaviour {
    private string IPAddress;
	  ConnectionConfig config;
	  HostTopology topology;
    Thread serverThread;
    Thread clientThread;
    LocalIP localIP;
    AsynchronousSocketListener listener;
    private string localIPAddress;
    private int port;

    public void Start() {
        localIP = new LocalIP();
        listener = new AsynchronousSocketListener();
        localIPAddress = localIP.Address();
        port = localIP.Port();
        Debug.Log("World initialized with server on IP " + localIPAddress + " through port " + port);

        serverThread = new Thread(new ThreadStart(listener.StartListening));
        serverThread.Start();
    }

    //This is the function that serializes the message before sending it
    void sendMessage(string textInput) {

    }

    private void Update() {
    }
}