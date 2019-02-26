using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames
{
    public enum MinigameState
    {
        Idle,       // Ready to start but not doing anything
        Started,    // Currently in progress
        Completed,  // Completed but not finalised
        Done        //
    }

    public interface Minigame
    {
        // Fields.
        Vector3 epicentre { get; set; }
        MinigameState state { get; set; }
        List<GameObject> objects { get; set; }
        GameObject areaHighlight { get; set; }
        int collectedAmount { get; set; }
        TextManager textManager { get; set; }
        int amount { get; set; }
        string resourceType { get; set; }
        int uniqueID { get; set; }

        // Implementation Specific Functions.
        void OnStart();
        void Update();
    }

    public static class MinigameHelper
    {
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

        public static void Start(this Minigame minigame)
        {
            MonoBehaviour.Destroy(minigame.areaHighlight);
            minigame.state = MinigameState.Started;

            minigame.OnStart();
        }
    }
}
