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
using Minigames;
using System.Collections.Generic;
using Utils;

namespace Server
{
    public class BlueprintServer : MonoBehaviour
    {
        public GameObject cursor;
        public TextManager textManager;

        private LocalIP localIP;
        private SocketListener listener;
        private ServerState serverState;
        private MinigameManager minigameManager;
#if NETFX_CORE
#else
        private Thread serverThread;
#endif

        public void Start()
        {
            // Initialize the synchronous socket listener
            localIP = new LocalIP();
            serverState = new ServerState();
            listener = new SocketListener(localIP, serverState);
            Debug.Log("BlueprintServer: World initialized with server on IP " + localIP.Address() + " through port " + localIP.Port());
            textManager = GameObject.Find("TextManager").GetComponent(typeof(TextManager)) as TextManager;

            textManager.RequestText(localIP.Address());
            minigameManager = new MinigameManager(serverState);

#if NETFX_CORE
            IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
                (workItem) =>
            {
                listener.StartListening();
            });
#else
            // Uncomment for full unity server testing
            //serverThread = new Thread(new ThreadStart(listener.StartListening));
            //serverThread.Start();

            // Uncomment for object spawn testing
            //Debug.Log(serverState.ProcessInstruction("I;000;00000.00;-0005.00;Wood;004"));
#endif
        }

        public void Update()
        {
            foreach (KeyValuePair<int, Spawnable> entry in serverState.spawnables)
            {
                Spawnable spawnable = entry.Value;
                if (!spawnable.spawned)
                {
                    Debug.Log("BlueprintServer: loading " + spawnable.type);
                    textManager.RequestReset();
                    minigameManager.PlaceMinigame(spawnable.type, spawnable.GetPosition(), spawnable.amount, spawnable.uniqueID);
                    spawnable.spawned = true;
                }
            }

            minigameManager.Update();

            if (Input.GetKeyDown("u"))
            {
                Debug.Log(serverState.ProcessInstruction("I;000;00000.00;-0005.00;Wood;004"));
            }
            if (Input.GetKeyDown("o"))
            {
                Debug.Log(serverState.ProcessInstruction("I;001;00020.00;-0005.00;Wood;003"));
            }
        }
    }
}
