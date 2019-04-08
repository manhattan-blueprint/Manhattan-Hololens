using System.Collections.Generic;
using UnityEngine;
using Utils;
using Utils.HoloToolkit.Unity;
using UnityEngine.UI;

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
    public interface IMinigame
    {
        Vector3 Epicentre { get; set; }                     // Center of the minigame zone.
        MinigameState State { get; set; }                   // Current state of the minigame.
        MinigameState LastState { get; set; }               // State of the minigame last tick.
        TextManager TextManager { get; set; }               // The informative text manager, to show the amount collected or how to collect.
        GestureInfoManager GestureInfoManager { get; set; } // The informative gesture manager, to show how to collect a resource.
        List<GameObject> Objects { get; set; }              // Objects to collect in the minigame.
        GameObject AreaHighlight { get; set; }              // The big hexagonal pillar showing the center of the minigame from a distance.
        GameObject Floor { get; set; }                      // The floor (to prevent objects falling through).
        int CollectedAmount { get; set; }                   // The amount of resources collected during the minigame (original amount minus failed collections).
        int Amount { get; set; }                            // The amount of resources to spawn at the start.
        int UniqueID { get; set; }                          // The unique ID of the instruction this minigame is attached to.
        int TimeLeft { get; set; }                          // The amount of time left.
        string ResourceName { get; set; }                   // The name of the object used when collecting.
        string FileName { get; set; }                       // The name of the object used for files, for the spawnables and area highlights.

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
        public static void Initialize(this IMinigame minigame, int game, Vector3 epicentre, int amount, int uniqueID, TextManager textManager, GestureInfoManager gestureInfoManager)
        {
            minigame.Epicentre = epicentre;
            minigame.State = MinigameState.Idle;
            minigame.Objects = new List<GameObject>();
            minigame.Amount = amount;
            minigame.CollectedAmount = 0;
            minigame.UniqueID = uniqueID;
            minigame.TextManager = textManager;

            switch (game)
            {
                case 1:
                    minigame.FileName = "Pine";
                    minigame.ResourceName = "wood";
                    break;

                case 2:
                    minigame.FileName = "Rocks";
                    minigame.ResourceName = "stone";
                    break;

                case 3:
                    minigame.FileName = "Clay";
                    minigame.ResourceName = "clay";
                    break;

                case 4:
                    minigame.FileName = "IronOre";
                    minigame.ResourceName = "iron ore";
                    break;

                case 5:
                    minigame.FileName = "CopperOre";
                    minigame.ResourceName = "copper ore";
                    break;

                case 6:
                    minigame.FileName = "RubberTree";
                    minigame.ResourceName = "rubber";
                    break;

                case 7:
                    minigame.FileName = "DiamondOre";
                    minigame.ResourceName = "diamond";
                    break;

                case 8:
                    minigame.FileName = "Sand";
                    minigame.ResourceName = "sand";
                    break;

                case 9:
                    minigame.FileName = "SilicaOre";
                    minigame.ResourceName = "silica ore";
                    break;

                case 10:
                    minigame.FileName = "QuartzOre";
                    minigame.ResourceName = "quartz";
                    break;

                default:
                    Debug.Log("Minigame: Error decoding, spawning wood anyway.");
                    minigame.FileName = "Pine";
                    minigame.ResourceName = "wood";
                    break;
            }

            //minigame.AreaHighlight = MonoBehaviour.Instantiate(Resources.Load("Areas/" + minigame.FileName, typeof(GameObject))) as GameObject;
            minigame.AreaHighlight = MonoBehaviour.Instantiate(Resources.Load("Areas/Wood", typeof(GameObject))) as GameObject;

            // Set to 50 to re-enable a pillar spawning directly in front.
            minigame.AreaHighlight.transform.position = minigame.Epicentre + new Vector3(0.0f, 1000.0f, 0.0f);

            minigame.GestureInfoManager = gestureInfoManager;
        }

        /// <summary>
        /// Officially starts the minigame.
        /// </summary>
        /// <param name="minigame"></param>
        public static void Start(this IMinigame minigame)
        {
            MyAnimation areaHighlightAnimation = minigame.AreaHighlight.AddComponent<MyAnimation>() as MyAnimation;
            areaHighlightAnimation.StartAnimation(Anims.moveAccelerate, new Vector3(0.0f, 200.0f, 0.0f), 0.000001f, true);

            MyDirectionIndicator indicator = minigame.AreaHighlight.GetComponent<MyDirectionIndicator>();
            indicator.HideIndicators();
            MonoBehaviour.Destroy(indicator);

            minigame.State = MinigameState.Started;

            minigame.Floor = MonoBehaviour.Instantiate(Resources.Load("Floor", typeof(GameObject))) as GameObject;
            minigame.Floor.transform.position = minigame.Epicentre + new Vector3(0.0f, -0.8f, 0.0f);

            minigame.OnStart();

            minigame.TextManager.RequestTimer(30, 1.0f);

            ShowLookAround(minigame.ResourceName);
        }

        /// <summary>
        /// Should be called inline with MonoBehaviour Update.
        /// </summary>
        /// <param name="minigame"></param>
        public static void Update(this IMinigame minigame)
        {
            minigame.OnUpdate();
            if (minigame.TextManager.GetTimeLeft() <= 0 || minigame.CollectedAmount >= minigame.Amount)
            {
                Complete(minigame);
                return;
            }

            if (minigame.TextManager.GetTimeLeft() <= 27)
                HideLookAround();
            if (minigame.TextManager.GetTimeLeft() <= 20 && minigame.TextManager.GetTimeLeft() >= 17)
                minigame.GestureInfoManager.RequestText("Look around");
            else if (minigame.TextManager.GetTimeLeft() <= 10 && minigame.TextManager.GetTimeLeft() >= 8)
                minigame.GestureInfoManager.RequestText("Check behind you");
            else if (minigame.TextManager.GetTimeLeft() <= 5 && minigame.TextManager.GetTimeLeft() >= 1)
                minigame.GestureInfoManager.RequestText("Quickly! " + (minigame.Amount - minigame.CollectedAmount) + " more to go");
            else if (minigame.CollectedAmount >= 1 && minigame.TextManager.GetTimeLeft() >= 1)
                minigame.GestureInfoManager.RequestText(minigame.CollectedAmount + " out of " + minigame.Amount + " collected");
        }

        /// <summary>
        /// Notify other states of final minigame result and end the game.
        /// </summary>
        /// <param name="minigame"></param>
        public static void Complete(this IMinigame minigame)
        {

            foreach (var item in minigame.Objects)
                MonoBehaviour.Destroy(item);

            minigame.TextManager.RequestTimerStop();
            minigame.GestureInfoManager.RequestReset();
            minigame.GestureInfoManager.RequestHide();
            minigame.OnComplete();
            minigame.State = MinigameState.Completed;
            MonoBehaviour.Destroy(minigame.Floor);

            // Uncomment to cause fail if not enough resources collected.
            //if (minigame.Amount >= minigame.CollectedAmount)
            minigame.TextManager.RequestText("You collected " + minigame.CollectedAmount + " " + minigame.ResourceName + "!", 3.0f);
            //else
            //    minigame.TextManager.RequestText("...Failure...", 3.0f);
            //    minigame.CollectedAmount = 0;
        }

        public static void ShowLookAround(string name)
        {
            GameObject.Find("LookAroundText").GetComponent<TextMesh>().text = "Look Around for nearby " + name;
        }

        public static void HideLookAround()
        {
            GameObject.Find("LookAroundText").GetComponent<TextMesh>().text = "";
        }
    }
}
