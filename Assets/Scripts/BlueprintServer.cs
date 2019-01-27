/*
Manages all the server and interacts with the game.
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
using System.Linq;
using System;
using System.Collections.Generic;

// Useful for converting and storing messages into useful object data.
public struct SpawnInfo {
    public float xCo, yCo, zCo;
    public String type;

    public SpawnInfo(String inpContent) {
        inpContent = inpContent.Replace("\n", "");
        String[] temp = inpContent.Split(new string[] { ";" }, StringSplitOptions.None);
        xCo = float.Parse(temp[1], System.Globalization.CultureInfo.InvariantCulture);
        yCo = float.Parse(temp[2], System.Globalization.CultureInfo.InvariantCulture);
        zCo = float.Parse(temp[3], System.Globalization.CultureInfo.InvariantCulture);
        type = temp[4];
    }

    public void LogInfo() {
        Debug.Log("Coords: (" + xCo + ", " + yCo + ", " + zCo + "), Type: " + type);
    }
}

public class BlueprintServer : MonoBehaviour {
    private Thread serverThread;
    private LocalIP localIP;
    private AsynchronousSocketListener listener;
    private string localIPAddress;
    private int port;
    private ServerState serverState;
    private String greetMessage, connectedMessage;
    List<String> pastInstructions;
    HoloInteractive holoInteractive;

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

        pastInstructions = new List<String>();

        SpawnInfo spawnInfo = new SpawnInfo("I;3.12;1.37;4.32;wood");
        spawnInfo.LogInfo();
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
                    if (pastInstructions.Contains(content)) {
                        listener.Send(handler, content);
                    }
                    else if (content[0] == 'I') {
                        Debug.Log("Server: received instruction");
                        SpawnInfo spawnInfo = new SpawnInfo(content);
                        holoInteractive.SpawnObject(new Vector3(spawnInfo.xCo, spawnInfo.yCo, spawnInfo.zCo));
                        pastInstructions.Add(content);
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
