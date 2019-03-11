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
                stone.transform.position = epicentre + new Vector3(Random.Range(-1.2f, 1.2f),
                    CameraCache.Main.transform.position.y + 1.0f, Random.Range(-1.2f, 1.2f));
                HoloInteractive holoInteractive = stone.AddComponent<HoloInteractive>() as HoloInteractive;
                holoInteractive.SetAttributes(InteractType.Drag, 8, true);
                objects.Add(stone);
            }
            state = MinigameState.Started;

            bag = MonoBehaviour.Instantiate(Resources.Load("Bag", typeof(GameObject))) as GameObject;
            bag.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3.0f;
            bag.transform.position -= new Vector3(0.0f, Camera.main.transform.forward.y * 3.0f + 0.5f, 0.0f);
                //epicentre + new Vector3(0.0f, CameraCache.Main.transform.position.y + 0.5f, 0.0f);

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
                if (item.transform.position.y < floor.transform.position.y - 0.2f)
                {
                    item.transform.position = new Vector3(item.transform.position.x, 
                        floor.transform.position.y + 0.2f, item.transform.position.y);
                }

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

            // Make bag face towards player.
            //gameObject.transform.rotation = mainCamera.transform.rotation;
            //transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            //    mainCamera.transform.rotation * Vector3.up);
        }

        void Minigame.OnComplete()
        {
            MonoBehaviour.Destroy(bag);
            if (amount == collectedAmount)
                textManager.RequestText("You collected " + collectedAmount + " stone!", 3.0f);
            else
                textManager.RequestText("...Failure...", 3.0f);
        }
    }
}
