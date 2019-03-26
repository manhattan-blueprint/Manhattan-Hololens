using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using Utils;

namespace Minigames
{
    /// <summary>
    /// Tap items to make them shrink, shrink them completely to collect.
    /// </summary>
    public class ClickShrink : IMinigame
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

        /// <summary>
        /// Spawn in the resources, start the timer and show helpful text.
        /// </summary>
        void IMinigame.OnStart()
        {
            // Spawn in loads of trees and make them click to shrink.
            for (int i = 0; i < Amount; i++)
            {
                GameObject collectableObject = MonoBehaviour.Instantiate(Resources.Load("Objects/" + FileName, typeof(GameObject))) as GameObject;
                collectableObject.transform.position = Epicentre + new Vector3(Random.Range(-2.0f, 2.0f),
                    CameraCache.Main.transform.position.y + 0.3f, Random.Range(-2.0f, 2.0f));
                MyAnimation animation = collectableObject.AddComponent<MyAnimation>() as MyAnimation;
                animation.StartAnimation(Anims.grow, Vector3.zero, 10.0f);
                HoloInteractive holoInteractive = collectableObject.AddComponent<HoloInteractive>() as HoloInteractive;
                holoInteractive.SetAttributes(InteractType.ClickShrink, 4, false, 10.0f);
                Objects.Add(collectableObject);
            }
            GestureInfoManager.RequestShowTapInfo();
        }

        /// <summary>
        /// Check for updated state of objects.
        /// </summary>
        void IMinigame.OnUpdate()
        {
            foreach (var item in Objects)
            {
                HoloInteractive holoInteractive = item.GetComponent<HoloInteractive>();
                if (holoInteractive.interactState == InteractState.Hidden)
                {
                    Objects.Remove(item);
                    MonoBehaviour.Destroy(item);
                    CollectedAmount += 1;
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
        }
    }
}
