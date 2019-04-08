using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using Utils;

namespace Minigames
{
    /// <summary>
    /// Drag items to a bag to collect.
    /// </summary>
    public class BagDrag : IMinigame
    {
        public Vector3 Epicentre { get; set; }
        public MinigameState State { get; set; }
        public MinigameState LastState { get; set; }
        public TextManager TextManager { get; set; }
        public GestureInfoManager GestureInfoManager { get; set; }
        public List<GameObject> Objects { get; set; }
        public GameObject AreaHighlight { get; set; }
        public GameObject Floor { get; set; }
        public int CollectedAmount { get; set; }
        public int Amount { get; set; }
        public int UniqueID { get; set; }
        public int TimeLeft { get; set; }
        public string ResourceName { get; set; }
        public string FileName { get; set; }

        private GameObject bag;

        /// <summary>
        /// Spawn in the resources, start the timer and show helpful text.
        /// </summary>
        void IMinigame.OnStart()
        {
            this.Amount = Amount;
           
            /// <summary>
            /// Spawn in the resources, start the timer and show helpful text.
            /// </summary>
            for (int i = 0; i < Amount; i++)
            {
                GameObject collectableObject = MonoBehaviour.Instantiate(Resources.Load("Objects/" + FileName, typeof(GameObject))) as GameObject;
                collectableObject.transform.position = Epicentre + new Vector3(Random.Range(-1.5f, 1.5f),
                    CameraCache.Main.transform.position.y + 1.0f, Random.Range(-1.5f, 1.5f));
                HoloInteractive holoInteractive = collectableObject.AddComponent<HoloInteractive>() as HoloInteractive;
                holoInteractive.SetAttributes(InteractType.Drag, 8, true);
                Objects.Add(collectableObject);
            }
            State = MinigameState.Started;

            bag = MonoBehaviour.Instantiate(Resources.Load("Bag", typeof(GameObject))) as GameObject;
            bag.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3.0f;
            bag.transform.position -= new Vector3(0.0f, Camera.main.transform.forward.y * 3.0f + 0.5f, 0.0f);
            //epicentre + new Vector3(0.0f, CameraCache.Main.transform.position.y + 0.5f, 0.0f);

            MyAnimation animation = bag.AddComponent<MyAnimation>() as MyAnimation;
            animation.StartAnimation(Anims.oscillate, Vector3.zero);

            GestureInfoManager.RequestShowDragInfo();
        }

        /// <summary>
        /// Check for updated state of objects.
        /// </summary>
        void IMinigame.OnUpdate()
        {
            foreach (var item in Objects)
            {
                if (item.transform.position.y < Floor.transform.position.y - 0.2f)
                {
                    item.transform.position = new Vector3(item.transform.position.x,
                        Floor.transform.position.y + 0.2f, item.transform.position.y);
                }

                HoloInteractive holoInteractive = item.GetComponent<HoloInteractive>();
                if (Vector3.Distance(bag.transform.position, item.transform.position) < 0.5f)
                {
                    MonoBehaviour.Destroy(item);
                    CollectedAmount += 1;
                    Objects.Remove(item);
                    return;
                }
                if (holoInteractive.interactState == InteractState.Touched)
                {
                    GestureInfoManager.RequestHide();
                }
            }
        }
        
        /// <summary>
        /// Any completion handling custom for the minigame.
        /// </summary>
        void IMinigame.OnComplete()
        {
            MonoBehaviour.Destroy(bag);
        }
    }
}
