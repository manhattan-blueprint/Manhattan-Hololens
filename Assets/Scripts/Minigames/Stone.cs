/* Minigame for collecting wood. */

using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using Utils;

namespace Minigames
{
    public class Stone : Minigame
    {
        public Vector3 epicentre { get; set; }
        public MinigameState state { get; set; }
        public List<GameObject> objects { get; set; }
        public GameObject areaHighlight { get; set; }
        public int collectedAmount { get; set; }
        public TextManager textManager { get; set; }
        public int amount { get; set; }
        public string resourceType { get; set; }
        public int uniqueID { get; set; }

        void Minigame.OnStart()
        {
            this.amount = amount;

            MonoBehaviour.Destroy(areaHighlight);

            // Spawn in loads of trees and make them draggable.
            for (int i = 0; i < amount; i++)
            {
                GameObject tree = MonoBehaviour.Instantiate(Resources.Load("Objects/Stone", typeof(GameObject))) as GameObject;
                tree.transform.position = epicentre + new Vector3(Random.Range(-2.0f, 2.0f),
                    CameraCache.Main.transform.position.y, Random.Range(-2.0f, 2.0f));
                HoloInteractive holoInteractive = tree.AddComponent<HoloInteractive>() as HoloInteractive;
                holoInteractive.SetAttributes(InteractType.ClickShrink, 8);
                objects.Add(tree);
            }
            state = MinigameState.Started;
            textManager.RequestText("Put the rock in the sack!");
        }

        void Minigame.Update()
        {
            foreach (var item in objects)
            {
                HoloInteractive holoInteractive = item.GetComponent<HoloInteractive>();
                if (holoInteractive.interactState == InteractState.Touched)
                {
                    textManager.RequestReset();
                }
                if (holoInteractive.interactState == InteractState.Hidden)
                {
                    objects.Remove(item);
                    MonoBehaviour.Destroy(item);
                    collectedAmount += 1;
                    return;
                }
            }
        }
    }
}
