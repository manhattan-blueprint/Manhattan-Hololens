using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System.Linq;
using Utils;

namespace Minigames
{
    /// <summary>
    /// Minigame for collecting stone.
    /// </summary>
    public class Stone : Minigame
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
        public int uniqueID { get; set; }
        public GameObject floor { get; set; }
        public int timeLeft { get; set; }

        private GameObject bag;

        void Minigame.OnStart()
        {
            this.amount = amount;

            // Spawn in loads of trees and make them draggable.
            for (int i = 0; i < amount; i++)
            {
                GameObject stone = MonoBehaviour.Instantiate(Resources.Load("Objects/Rocks", typeof(GameObject))) as GameObject;
                stone.transform.position = epicentre + new Vector3(Random.Range(-2.0f, 2.0f),
                    CameraCache.Main.transform.position.y + 1.0f, Random.Range(-2.0f, 2.0f));
                HoloInteractive holoInteractive = stone.AddComponent<HoloInteractive>() as HoloInteractive;
                holoInteractive.SetAttributes(InteractType.Drag);
                objects.Add(stone);
            }
            state = MinigameState.Started;
            textManager.RequestText("Put the rocks in the sack!", 2.0f);

            bag = MonoBehaviour.Instantiate(Resources.Load("Bag", typeof(GameObject))) as GameObject;
            bag.transform.position = epicentre + new Vector3(0.0f, CameraCache.Main.transform.position.y + 0.8f, 0.0f);

            MyAnimation animation = bag.AddComponent<MyAnimation>() as MyAnimation;
            animation.StartAnimation(Anims.oscillate, Vector3.zero);

            gestureInfoManager.RequestShowDragInfo();
        }

        /// <summary>
        /// 
        /// </summary>
        void Minigame.OnUpdate()
        {
            foreach (var item in objects)
            {
                HoloInteractive holoInteractive = item.GetComponent<HoloInteractive>();
                if (Vector3.Distance(bag.transform.position, item.transform.position) < 0.5f)
                {
                    MonoBehaviour.Destroy(item);
                    collectedAmount += 1;
                    objects.Remove(item);
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
            MonoBehaviour.Destroy(bag);
            textManager.RequestText("You collected " + collectedAmount + " stone!", 3.0f);
        }
    }
}
