using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames
{
    /// <summary>
    /// Stores the current state of the minigame.
    /// </summary>
    public enum MinigameState
    {
        Idle,       // Ready to start but not doing anything
        Started,    // Currently in progress
        Timing,     // Currently in progress and timer started
        Completed   // Completed and ready to notify of completion
    }

    /// <summary>
    /// Template for all minigames.
    /// </summary>
    public interface Minigame
    {
        Vector3 epicentre { get; set; }                     // Center of the minigame zone.
        MinigameState state { get; set; }                   // Current state of the minigame.
        MinigameState lastState { get; set; }               // State of the minigame last tick.
        List<GameObject> objects { get; set; }              // Objects to collect in the minigame.
        GameObject areaHighlight { get; set; }              // The big hexagonal pillar showing the center of the minigame from a distance.
        int collectedAmount { get; set; }                   // The amount of resources collected during the minigame (original amount minus failed collections).
        TextManager textManager { get; set; }               // The informative text manager, to show the amount collected or how to collect.
        GestureInfoManager gestureInfoManager { get; set; } // The informative gesture manager, to show how to collect a resource
        int amount { get; set; }                            // The amount of resources to spawn at the start.
        string resourceType { get; set; }                   // The type of resources to spawn.
        int uniqueID { get; set; }                          // The unique ID of the instruction this minigame is attached to.
        GameObject floor { get; set; }                      // The floor (to prevent objects falling through).
        int timeLeft { get; set; }                          // The amount of time left.

        void OnStart();
        void OnUpdate();
        void OnComplete();
    }

    /// <summary>
    /// Implements some useful classes present in all minigames to prevent repetetive code.
    /// </summary>
    public static class MinigameHelper
    {
        /// <summary>
        /// Initializes the minigame.
        /// </summary>
        /// <param name="minigame"></param>
        /// <param name="game"></param>
        /// <param name="epicentre"></param>
        /// <param name="amount"></param>
        /// <param name="uniqueID"></param>
        /// <param name="textManager"></param>
        public static void Initialize(this Minigame minigame, string game, Vector3 epicentre, int amount, int uniqueID, TextManager textManager, GestureInfoManager gestureInfoManager)
        {
            minigame.epicentre = epicentre;
            minigame.state = MinigameState.Idle;
            minigame.objects = new List<GameObject>();
            minigame.amount = amount;
            minigame.collectedAmount = 0;
            minigame.resourceType = game;
            minigame.uniqueID = uniqueID;
            minigame.textManager = textManager;

            minigame.areaHighlight = MonoBehaviour.Instantiate(Resources.Load("Areas/" + game, typeof(GameObject))) as GameObject;
            minigame.areaHighlight.transform.position = minigame.epicentre;

            minigame.gestureInfoManager = gestureInfoManager;
        }

        /// <summary>
        /// Officially starts the minigame.
        /// </summary>
        /// <param name="minigame"></param>
        public static void Start(this Minigame minigame)
        {
            MonoBehaviour.Destroy(minigame.areaHighlight);
            minigame.state = MinigameState.Started;

            minigame.floor = MonoBehaviour.Instantiate(Resources.Load("Floor", typeof(GameObject))) as GameObject;
            minigame.floor.transform.position = minigame.epicentre + new Vector3(0.0f, -1.0f, 0.0f);
            
            minigame.textManager.RequestTimer(15);

            minigame.OnStart();
        }

        public static void Update(this Minigame minigame)
        {
            minigame.OnUpdate();
            if (minigame.textManager.GetTimeLeft() <= 0 || minigame.collectedAmount >= minigame.amount)
            {
                Complete(minigame);
            }
        }

        public static void Complete(this Minigame minigame)
        {
            foreach (var item in minigame.objects)
            {
                MonoBehaviour.Destroy(item);
            }
            minigame.textManager.RequestTimerStop();
            minigame.OnComplete();
            minigame.state = MinigameState.Completed;
            MonoBehaviour.Destroy(minigame.floor);
        }
    }
}
