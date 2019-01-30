/*
Manages all the server and interacts with the game.
Attach this script to a GameObject. Create a Text GameObject (Create>UI>Text)
and attach it to the My Text field in the Inspector of your GameObject. Press
the space bar in Play Mode to see the Text change.
*/
using UnityEngine;
using System.Threading;
using System;
using HoloToolkit.Unity;

// Useful for converting and storing messages into useful object data.
public struct SpawnInfo {
    private float xCo, yCo, zCo;
    public String type;

    public SpawnInfo(String inpContent) {
        inpContent = inpContent.Replace("\n", "");
        String[] temp = inpContent.Split(new string[] { ";" }, StringSplitOptions.None);
        xCo = float.Parse(temp[1], System.Globalization.CultureInfo.InvariantCulture);
        yCo = float.Parse(temp[2], System.Globalization.CultureInfo.InvariantCulture);
        zCo = float.Parse(temp[3], System.Globalization.CultureInfo.InvariantCulture);
        type = temp[4];
    }

    public Vector3 GetPosition() {
        return new Vector3(xCo, yCo, zCo);
    }

    public void LogInfo() {
        Debug.Log("SpawnInfo: Coords: (" + xCo + ", " + yCo + ", " + zCo + "), Type: " + type);
    }
}

public class BlueprintServer : MonoBehaviour {
    private Thread serverThread;
    private LocalIP localIP;
    private SocketListener listener;
    private ServerState serverState;

    public GameObject cursor;
    public TextMesh infoText;

    public void Start() {
        // Initialize the synchronous socket listener
        localIP = new LocalIP();
        serverState = new ServerState();
        listener = new SocketListener(localIP, serverState);
        Debug.Log("BlueprintServer: World initialized with server on IP " + localIP.Address() + " through port " + localIP.Port());

        infoText.text = localIP.Address();
        serverThread = new Thread(new ThreadStart(listener.StartListening));
        serverThread.Start();
    }

    public void Update() {
        String instruction = serverState.GetFreshSpawn();
        if (!string.Equals("", instruction)) {
            Debug.Log("BlueprintServer: Fresh spawn found! Instruction is " + instruction);
            SpawnInfo spawnInfo = new SpawnInfo(instruction);
            Spawn(spawnInfo.type, spawnInfo.GetPosition());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Vector3 position = new Vector3(0.0f, 0.0f, 3.5f);
            Spawn("wood", position);
        }
    }

    private void Spawn(String type, Vector3 position) {
        Debug.Log("BlueprintServer: loading " + type);
        infoText.text = "";
        GameObject gObject = Instantiate(Resources.Load(type, typeof(GameObject))) as GameObject;

        // Make it interactive
        HoloInteractive holoInteractive = gObject.AddComponent<HoloInteractive>() as HoloInteractive;
        holoInteractive.SetAttributes("wood", position);

        // // Add direction indicator
        // MyDirectionIndicator directionIndicator = gObject.AddComponent<MyDirectionIndicator>() as MyDirectionIndicator;
        // GameObject indicator = Instantiate(Resources.Load("direction_indicator", typeof(GameObject))) as GameObject;
        // directionIndicator.SetAttributes(indicator, cursor);
    }
}
