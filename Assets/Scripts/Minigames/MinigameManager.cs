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
        private bool spawned;
        private string currentGame;
        private BlueprintServer blueprintServer;
        private List<Minigame> minigames;
        public TextManager textManager;
        private ServerState serverState;

        public MinigameManager(ServerState serverState)
        {
            blueprintServer = GameObject.Find("Server").GetComponent(typeof(BlueprintServer)) as BlueprintServer;
            textManager = GameObject.Find("TextManager").GetComponent(typeof(TextManager)) as TextManager;
            minigames = new List<Minigame>();
            this.serverState = serverState;
        }

        public void PlaceMinigame(string game, Vector3 position, int amount, int uniqueID)
        {
            Debug.Log("New minigame being placed at " + position + " of game type " + game);

            Minigame minigame;

            switch (game)
            {
                case "Wood":
                    minigame = new Wood();
                    minigame.Initialize(game, position + new Vector3(0, -1.0f, 0), amount, uniqueID, textManager);
                    break;

                default:
                    Debug.Log("ERROR: no minigame decodeable from '" + game + "'; not proceeding with starting game");
                    return;
            }
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
                        if (Vector3.Distance(minigame.epicentre, CameraCache.Main.transform.position) < 3.0f)
                        {
                            minigame.Start();
                        }
                        break;

                    case MinigameState.Started:
                        minigame.Update();
                        break;

                    case MinigameState.Completed:
                        Debug.Log("Minigame Complete");
                        textManager.RequestText("You collected " + minigame.amount + " " + minigame.resourceType + "!", 2.0f);
                        minigames.Remove(minigame);
                        serverState.NotifyComplete(minigame.uniqueID);
                        return;
                }
            }
        }
    }
}
