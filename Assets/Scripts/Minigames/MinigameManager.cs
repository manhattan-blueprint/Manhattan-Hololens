/*
Manages minigames
*/

using UnityEngine;
using HoloToolkit.Unity;
using System.Collections.Generic;

namespace Minigames
{
    public class MinigameManager : MonoBehaviour
    {
        private bool spawned;
        private string currentGame;
        private BlueprintServer blueprintServer;
        private List<Minigame> minigames;

        public void Start()
        {
            blueprintServer = GameObject.Find("Server").GetComponent(typeof(BlueprintServer)) as BlueprintServer;
            minigames = new List<Minigame>();
        }

        public void PlaceMinigame(string instruction, string game, Vector3 position)
        {
            Debug.Log("New minigame being placed at " + position + " of game type " + game);

            Minigame minigame;

            switch (game)
            {
                case "woo":
                    minigame = new Wood(position, game);
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
                MinigameState state = minigame.GetState();
                switch (state)
                {
                    case MinigameState.Idle:
                        if (Vector3.Distance(minigame.GetEpicentre(), CameraCache.Main.transform.position) < 3.0f)
                        {
                            // This quantity is a placeholder for now; could be directed by the phone.
                            int quantity = 10 /*Random.Range(5, 15)*/;

                            minigame.Start(quantity);
                        }
                        break;

                    case MinigameState.Started:
                        minigame.Update();
                        break;

                    case MinigameState.Completed:
                        minigame.Finish();
                        break;
                }
            }
        }
    }
}
