using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System.Linq;
using Utils;

namespace Minigames
{
    /// <summary>
    /// Minigame for collecting wood.
    /// </summary>
    public class Wood : Minigame
    {
        public Vector3 epicentre { get; set; }
        public MinigameState state { get; set; }
        public MinigameState lastState { get; set; }
        public List<GameObject> objects { get; set; }
        public GameObject areaHighlight { get; set; }
        public int collectedAmount { get; set; }
        public TextManager textManager { get; set; }
        public GestureInfoManager gestureInfoManager { get; set; }
        public int amount { get; set; }
        public string resourceType { get; set; }
        public int uniqueID { get; set; }
        public GameObject floor { get; set; }
        public int timeLeft { get; set; }

        void Minigame.OnStart()
        {
            // Spawn in loads of trees and make them click to shrink.
            for (int i = 0; i < amount; i++)
            {
                GameObject tree = MonoBehaviour.Instantiate(Resources.Load("Objects/tree", typeof(GameObject))) as GameObject;
                tree.transform.position = epicentre + new Vector3(Random.Range(-2.0f, 2.0f),
                    CameraCache.Main.transform.position.y + 1.0f, Random.Range(-2.0f, 2.0f));
                HoloInteractive holoInteractive = tree.AddComponent<HoloInteractive>() as HoloInteractive;
                holoInteractive.SetAttributes(InteractType.ClickShrink, 4);
                objects.Add(tree);
            }
            textManager.RequestText("Chop the trees down!", 2.0f);
            gestureInfoManager.RequestShowTapInfo();
        }

        void Minigame.OnUpdate()
        {
            foreach (var item in objects)
            {
                HoloInteractive holoInteractive = item.GetComponent<HoloInteractive>();
                if (holoInteractive.interactState == InteractState.Hidden)
                {
                    objects.Remove(item);
                    MonoBehaviour.Destroy(item);
                    collectedAmount += 1;
                    return;
                }
                if (holoInteractive.interactState == InteractState.Touched)
                {
                    gestureInfoManager.RequestHide();
                }
            }
        }

        void Minigame.OnComplete()
        {
            textManager.RequestText("You collected " + collectedAmount + " wood!", 3.0f);
        }
    }
}
