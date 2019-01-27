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
using System.Text;
using System;

public class BlueprintServer : MonoBehaviour {
    private Thread serverThread;
    private LocalIP localIP;
    private AsynchronousSocketListener listener;
    private string localIPAddress;
    private int port;
    private ServerState serverState;
    private String greetMessage;
    private String connectedMessage;

    public void Start() {
        localIP = new LocalIP();
        serverState = new ServerState();
        listener = new AsynchronousSocketListener(localIP, serverState);
        localIPAddress = localIP.Address();
        port = localIP.Port();
        Debug.Log("World initialized with server on IP " + localIPAddress + " through port " + port);

        greetMessage = "hello_blueprint";
        connectedMessage = "connected_blueprint";
        serverThread = new Thread(new ThreadStart(listener.StartListening));
        serverThread.Start();
    }

    public void Update() {
        String content = serverState.GetContent();
        int state = serverState.GetState();
        Socket handler = serverState.GetSocket();
        if (!string.Equals("", content)) {
            Debug.Log("Server: read " + content);
            switch (state) {
                case 0:
                    if (string.Equals(content, greetMessage + "\n")) {
                        listener.Send(handler, greetMessage);
                        Debug.Log("Server: swapping to state 'IDLE_IP'");
                        serverState.ToggleState();
                    }
                    break;
                case 1:
                    if (content[0] == 'I') {
                        Debug.Log("Server: received instruction");
                    }
                    else {
                        Debug.Log("Server: unexpected message received");
                    }
                    break;
                default:
                    break;
            }
            serverState.RecycleContent();
        }
    }
}
