﻿/* Minigame for collecting wood. */

using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System.Linq;

namespace Minigames
{
    public class Wood : Minigame
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
            // Spawn in loads of trees and make them click to shrink.
            for (int i = 0; i < amount; i++)
            {
                GameObject tree = MonoBehaviour.Instantiate(Resources.Load("Objects/tree", typeof(GameObject))) as GameObject;
                tree.transform.position = epicentre + new Vector3(Random.Range(-2.0f, 2.0f), 
                    CameraCache.Main.transform.position.y, Random.Range(-2.0f, 2.0f));
                HoloInteractive holoInteractive = tree.AddComponent<HoloInteractive>() as HoloInteractive;
                holoInteractive.SetAttributes(InteractType.ClickShrink, 4);
                objects.Add(tree);
            }
            textManager.RequestText("Chop the trees down!");
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
            if (!objects.Any())
            {
                state = MinigameState.Completed;
            }
        }
    }
}
