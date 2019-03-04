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
        public int amount { get; set; }
        public string resourceType { get; set; }
        public int uniqueID { get; set; }
        public GameObject floor { get; set; }
        public int timeLeft { get; set; }

        private GameObject bag;

        void Minigame.OnStart()
        {
            this.amount = amount;

            MonoBehaviour.Destroy(areaHighlight);

            // Spawn in loads of trees and make them draggable.
            for (int i = 0; i < amount; i++)
            {
                GameObject stone = MonoBehaviour.Instantiate(Resources.Load("Objects/Rocks", typeof(GameObject))) as GameObject;
                stone.transform.position = epicentre + new Vector3(Random.Range(-2.0f, 2.0f),
                    CameraCache.Main.transform.position.y, Random.Range(-2.0f, 2.0f));
                HoloInteractive holoInteractive = stone.AddComponent<HoloInteractive>() as HoloInteractive;
                holoInteractive.SetAttributes(InteractType.Drag);
                objects.Add(stone);
            }
            state = MinigameState.Started;
            textManager.RequestText("Put the rocks in the sack!", 2.0f);

            bag = MonoBehaviour.Instantiate(Resources.Load("Bag", typeof(GameObject))) as GameObject;
            bag.transform.position = epicentre + new Vector3(0.0f, CameraCache.Main.transform.position.y + 0.8f, 0.0f);
        }

        void Minigame.OnUpdate()
        {
            foreach (var item in objects)
            {
                HoloInteractive holoInteractive = item.GetComponent<HoloInteractive>();
                if (holoInteractive.interactState == InteractState.Idle)
                {
                    if (Vector3.Distance(bag.transform.position, item.transform.position) < 0.5f)
                    {
                        objects.Remove(item);
                        MonoBehaviour.Destroy(item);
                        collectedAmount += 1;
                    }
                }
                else if (holoInteractive.interactState == InteractState.Touched)
                {
                    state = MinigameState.Timing;
                }
            }
        }

        void Minigame.OnComplete()
        {
        }
    }
}
