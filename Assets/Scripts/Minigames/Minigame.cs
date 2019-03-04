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
        Completed  // Completed and ready to notify of completion
    }

    /// <summary>
    /// Template for all minigames.
    /// </summary>
    public interface Minigame
    {
        Vector3 epicentre { get; set; }         // Center of the minigame zone.
        MinigameState state { get; set; }       // Current state of the minigame.
        List<GameObject> objects { get; set; }  // Objects to collect in the minigame.
        GameObject areaHighlight { get; set; }  // The big hexagonal pillar showing the center of the minigame from a distance.
        int collectedAmount { get; set; }       // The amount of resources collected during the minigame (original amount minus failed collections).
        TextManager textManager { get; set; }   // The informative text manager, to show the amount collected or how to collect.
        int amount { get; set; }                // The amount of resources to spawn at the start.
        string resourceType { get; set; }       // The type of resources to spawn.
        int uniqueID { get; set; }              // The unique ID of the instruction this minigame is attached to.
        
        void OnStart();
        void Update();
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
        public static void Initialize(this Minigame minigame, string game, Vector3 epicentre, int amount, int uniqueID, TextManager textManager)
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

        }

        /// <summary>
        /// Officially starts the minigame.
        /// </summary>
        /// <param name="minigame"></param>
        public static void Start(this Minigame minigame)
        {
            MonoBehaviour.Destroy(minigame.areaHighlight);
            minigame.state = MinigameState.Started;

            minigame.OnStart();
        }
    }
}
