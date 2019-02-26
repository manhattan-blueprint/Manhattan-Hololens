/* Minigame for collecting wood. */

using System.Collections.Generic;
using UnityEngine;

namespace Minigames
{
    public class Wood : Minigame
    {
        private int spawnQuantity;
        private List<GameObject> objects;
        private GameObject successBox;
        private MinigameState state;
        private GameObject areaHighlight;
        private int numCollected;
        private Vector3 epicentre;

        public Wood(Vector3 epicenter, string type)
        {
            this.epicentre = epicenter;
            state = MinigameState.Idle;
            objects = new List<GameObject>();

            areaHighlight = MonoBehaviour.Instantiate(Resources.Load(type, typeof(GameObject))) as GameObject;
            areaHighlight.transform.position = epicenter;
        }
        
        void Minigame.Start(int spawnQuantity)
        {
            this.spawnQuantity = spawnQuantity;

            MonoBehaviour.Destroy(areaHighlight);

            // Spawn in loads of trees and make them draggable.
            for (int i = 0; i < spawnQuantity; i++)
            {
                GameObject tree = MonoBehaviour.Instantiate(Resources.Load("tree", typeof(GameObject))) as GameObject;
                tree.transform.position = epicentre + new Vector3(Random.Range(0.0f, 3.0f), 0.0f, Random.Range(0.0f, 3.0f));
                HoloInteractive holoInteractive = tree.AddComponent<HoloInteractive>() as HoloInteractive;
                holoInteractive.SetAttributes(InteractType.Drag);
                objects.Add(tree);
            }
            state = MinigameState.Started;

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(0, 1.5f, 0);
        }
        
        void Minigame.Update()
        {

        }
        
        int Minigame.Finish()
        {
            numCollected = 10;
            return numCollected;
        }

        MinigameState Minigame.GetState()
        {
            return state;
        }

        Vector3 Minigame.GetEpicentre()
        {
            return epicentre;
        }
    }
}
