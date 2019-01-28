/*
Manages all the server and interacts with the game.
Attach this script to a GameObject. Create a Text GameObject (Create>UI>Text)
and attach it to the My Text field in the Inspector of your GameObject. Press
the space bar in Play Mode to see the Text change.
*/
using UnityEngine;
using System.Threading;
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
    private SynchronousSocketListener listener;
    private ServerState serverState;
    private String greetMessage, connectedMessage;
    HoloInteractive holoInteractive;

    public void Start() {
        localIP = new LocalIP();
        serverState = new ServerState();
        listener = new SynchronousSocketListener(localIP, serverState);
        Debug.Log("World initialized with server on IP " + localIP.Address() + " through port " + localIP.Port());

        greetMessage = "hello_blueprint";
        connectedMessage = "connected_blueprint";
        serverThread = new Thread(new ThreadStart(listener.StartListening));
        serverThread.Start();

        SpawnInfo spawnInfo = new SpawnInfo("I;3.12;1.37;4.32;wood");
        spawnInfo.LogInfo();
    }

    public void Update() {
        String instruction = serverState.GetFreshSpawn();
        if (!string.Equals("", instruction)) {
            SpawnInfo spawnInfo = new SpawnInfo(instruction);
            holoInteractive.SpawnObject(new Vector3(spawnInfo.xCo, spawnInfo.yCo, spawnInfo.zCo));
        }
    }
}
