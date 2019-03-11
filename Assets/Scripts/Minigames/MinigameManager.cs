/*
Manages minigames
*/

using UnityEngine;
using HoloToolkit.Unity;
using System.Collections.Generic;
using Server;
using Utils;

namespace Minigames
{
    public class MinigameManager
    {
        private BlueprintServer blueprintServer;
        private List<Minigame> minigames;
        public TextManager textManager;
        public GestureInfoManager gestureInfoManager;
        private ServerState serverState;

        public MinigameManager(ServerState serverState)
        {
            blueprintServer = GameObject.Find("Server").GetComponent(typeof(BlueprintServer)) as BlueprintServer;
            textManager = GameObject.Find("TextManager").GetComponent(typeof(TextManager)) as TextManager;
            gestureInfoManager = GameObject.Find("GestureInfoManager").GetComponent(typeof(GestureInfoManager)) as GestureInfoManager;
            minigames = new List<Minigame>();
            this.serverState = serverState;
        }

        public void PlaceMinigame(int game, Vector3 position, int amount, int uniqueID)
        {
            Debug.Log("New minigame being placed at " + position + " of game type " + game + " with unique ID of " + uniqueID);

            Minigame minigame;

            // This is the only place where strings from instructions are converted to minigames
            switch (game)
            {
                case 1: minigame = new Wood(); break;
                case 2: minigame = new Stone(); break;
                //case "Clay": minigame = new Clay(); break;
                //case "Sand": minigame = new Sand(); break;
                //case "Iron": minigame = new IronOre(); break;
                //case "Coal": minigame = new Coal(); break;
                //case "Copp": minigame = new CopperOre(); break;
                //case "Rubb": minigame = new Rubber(); break;
                //case "Sili": minigame = new SilicaOre(); break;
                //case "Alum": minigame = new AluminiumOre(); break;
                //case "Quar": minigame = new Quartz(); break;

                default:
                    Debug.Log("ERROR: no minigame decodeable from '" + game + "'; not proceeding with starting game");
                    return;
            }
            minigame.Initialize(game, position + new Vector3(0, -1.0f, 0), amount, uniqueID, textManager, gestureInfoManager);
            minigames.Add(minigame);
        }

        public void Update()
        {
            foreach (var minigame in minigames)
            {
                MinigameState state = minigame.state;
                switch (state)
                {
                    case MinigameState.Idle:
                        // Set to 4 to reenable pillars
                        if (Vector3.Distance(minigame.epicentre, CameraCache.Main.transform.position) < 8.0f)
                        {
                            minigame.Start();
                        }
                        break;

                    case MinigameState.Started:
                        minigame.Update();
                        break;

                    case MinigameState.Timing:
                        minigame.Update();
                        break;

                    case MinigameState.Completed:
                        Debug.Log("Minigame Complete");
                        serverState.NotifyComplete(minigame.uniqueID, minigame.collectedAmount);
                        minigames.Remove(minigame);
                        return;
                }
            }
        }
    }
}
