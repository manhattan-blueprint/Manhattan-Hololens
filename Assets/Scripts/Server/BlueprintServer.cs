#if NETFX_CORE
using Windows.Foundation;
#else
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
#endif

using UnityEngine;
using Minigames;
using System.Collections.Generic;
using Utils;

namespace Server
{
    /// <summary>
    /// Manages the interaction between the low level socket listener and the game.
    /// </summary>
    public class BlueprintServer : MonoBehaviour
    {
        [Tooltip("Cursor object the user sees.")]
        public GameObject cursor;

        [Tooltip("Script in charge of managing the information text.")]
        public TextManager textManager;

        private LocalIP localIP;
        private SocketListener listener;
        private ServerState serverState;
        private MinigameManager minigameManager;
#if NETFX_CORE
#else
        private Thread serverThread;
#endif

        /// <summary>
        /// Automatically called when the Unity scene is made, as described by MonoBehaviour.
        /// </summary>
        public void Start()
        {
            localIP = new LocalIP();
            serverState = new ServerState();
            listener = new SocketListener(localIP, serverState);
            Debug.Log("BlueprintServer: World initialized with server on IP " + localIP.Address() + " through port " + localIP.Port());
            textManager = GameObject.Find("TextManager").GetComponent(typeof(TextManager)) as TextManager;

            textManager.RequestText(localIP.Address());
            minigameManager = new MinigameManager(serverState);

#if NETFX_CORE
            // Runs a socket listener asynchronously.
            IAsyncAction asyncAction = Windows.System.Threading.ThreadPool.RunAsync(
                (workItem) =>
            {
                listener.StartListening();
            });
#else
            // Note: uncommenting code for testing in general is bad, but in this case is the
            // simplest and extremely effective way for developing; when on the desktop I only
            // need to be testing the minigames and this lets me spawn them in easily.

            // Uncomment for full server (unity version only)
            //serverThread = new Thread(new ThreadStart(listener.StartListening));
            //serverThread.Start();

            // Uncomment to spawn a single object (unity version only)
            Debug.Log(serverState.ProcessInstruction("I;000;00000.00;00004.00;Ston;004"));
            Debug.Log(serverState.ProcessInstruction("I;000;00004.00;00004.00;Wood;004"));
#endif
        }

        /// <summary>
        /// Automatically called every scene update by Unity, as described by MonoBehaviour.
        /// </summary>
        public void Update()
        {
            foreach (KeyValuePair<int, Spawnable> entry in serverState.GetSpawnables())
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
        }
    }
}
