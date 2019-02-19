/*
Manages all the server and interacts with the game.
Attach this script to a GameObject. Create a Text GameObject (Create>UI>Text)
and attach it to the My Text field in the Inspector of your GameObject. Press
the space bar in Play Mode to see the Text change.
*/

#if NETFX_CORE
using Windows.Foundation;
using System.Text.RegularExpressions;
#else
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
#endif

using UnityEngine;
using HoloToolkit.Unity;


// Useful for converting and storing messages into useful object data.
public struct SpawnInfo {
    private float xCo, yCo, zCo;
    public string type;

    public SpawnInfo(string inpContent) {
        inpContent = inpContent.Replace("\n", "");
#if NETFX_CORE
            string[] temp = Regex.Split(inpContent, ";");
#else
            string[] temp = inpContent.Split(new string[] { ";" }, StringSplitOptions.None);
#endif
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
    public GameObject cursor;
    public TextMesh infoText;

    private GameObject areaHighlight;
    private LocalIP localIP;
    private SocketListener listener;
    private ServerState serverState;
    private MinigameManager minigameManager;
#if NETFX_CORE
#else
    private Thread serverThread;
#endif

    public void Start() {
        // Initialize the synchronous socket listener
        localIP = new LocalIP();
        serverState = new ServerState();
        listener = new SocketListener(localIP, serverState);
        Debug.Log("BlueprintServer: World initialized with server on IP " + localIP.Address() + " through port " + localIP.Port());

        infoText.text = localIP.Address();
        minigameManager = GameObject.Find("Minigame").GetComponent(typeof(MinigameManager)) as MinigameManager;

#if NETFX_CORE
        IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
            (workItem) =>
        {
            listener.StartListening();
        });
#else
        //serverThread = new Thread(new ThreadStart(listener.StartListening));
        //serverThread.Start();

        serverState.AddInstruction("I;0.0;0.0;3.0;wood");
#endif
    }

    public void Update() {
        if (minigameManager.complete)
        {
            string instruction = serverState.GetFreshSpawn();
            if (!string.Equals("", instruction))
            {
                Debug.Log("BlueprintServer: Fresh spawn found! Instruction is " + instruction);
                SpawnInfo spawnInfo = new SpawnInfo(instruction);
                Spawn(instruction, spawnInfo.type, spawnInfo.GetPosition());
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Vector3 position = new Vector3(0.0f, 0.0f, 3.5f);
                Spawn("Test Minigame", "wood", position);
            }
        }

        if (areaHighlight)
        {
            Camera mainCamera = CameraCache.Main;
            Vector3 newPos = areaHighlight.transform.position;
            newPos[1] = mainCamera.transform.position.y;
            areaHighlight.transform.position = newPos;
        }
    }

    private void Spawn(string instruction, string type, Vector3 position) {
        Debug.Log("BlueprintServer: loading " + type);
        infoText.text = "";
        areaHighlight = Instantiate(Resources.Load(type, typeof(GameObject))) as GameObject;
        areaHighlight.transform.position = position;

        minigameManager.StartMinigame(instruction, type, position);
    }

    public void HideAreaHighlight()
    {
        Destroy(areaHighlight);
    }
}
