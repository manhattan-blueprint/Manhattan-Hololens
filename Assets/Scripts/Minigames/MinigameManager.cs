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
    /// <summary>
    /// Manages all minigame instances.
    /// </summary>
    public class MinigameManager
    {
        [Tooltip("The main text manager of the game.")]
        public TextManager textManager;

        [Tooltip("The gesture information manager of the game.")]
        public GestureInfoManager gestureInfoManager;

        private BlueprintServer blueprintServer;
        private List<IMinigame> minigames;
        private ServerState serverState;

        public MinigameManager(ServerState serverState)
        {
            blueprintServer = GameObject.Find("Server").GetComponent(typeof(BlueprintServer)) as BlueprintServer;
            textManager = GameObject.Find("TextManager").GetComponent(typeof(TextManager)) as TextManager;
            gestureInfoManager = GameObject.Find("GestureInfoManager").GetComponent(typeof(GestureInfoManager)) as GestureInfoManager;
            minigames = new List<IMinigame>();
            this.serverState = serverState;
        }

        /// <summary>
        /// Places a new minigame in the world.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="position"></param>
        /// <param name="amount"></param>
        /// <param name="uniqueID"></param>
        public void PlaceMinigame(int game, Vector3 position, int amount, int uniqueID)
        {
            Debug.Log("New minigame being placed at " + position + " of game type " + game + " with unique ID of " + uniqueID);

            IMinigame minigame = new ClickShrink();

            //// This is the only place where strings from instructions are converted to minigames
            //switch (game)
            //{
            //    case 1: minigame = new ClickShrink(); break;
            //    case 2: minigame = new BagDrag(); break;

            //    default:
            //        Debug.Log("ERROR: no minigame decodeable from '" + game + "'; not proceeding with starting game");
            //        return;
            //}

            minigame.Initialize(game, position + new Vector3(0, -1.0f, 0), amount, uniqueID, textManager, gestureInfoManager);
            minigames.Add(minigame);
        }

        /// <summary>
        /// Checks for updates from the minigames, should be called inline with MonoBehaviour.Update().
        /// </summary>
        public void Update()
        {
            foreach (var minigame in minigames)
            {
                MinigameState state = minigame.State;
                switch (state)
                {
                    case MinigameState.Idle:
                        // Set to 4 to reenable pillars
                        if (Vector3.Distance(minigame.Epicentre, CameraCache.Main.transform.position) < 8.0f)
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
                        serverState.NotifyComplete(minigame.UniqueID, minigame.CollectedAmount);
                        minigames.Remove(minigame);
                        return;
                }
            }
        }
    }
}
