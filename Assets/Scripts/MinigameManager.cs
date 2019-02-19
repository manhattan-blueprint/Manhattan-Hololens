/*
Manages minigames
*/

using UnityEngine;
using HoloToolkit.Unity;


public class MinigameManager : MonoBehaviour {
    public bool complete;
    private bool spawned;
    private string currentInstruction;
    private string currentGame;
    private BlueprintServer blueprintServer;

    // TODO: Make these dynamically controlled lists for multiple objects.
    GameObject gObject;
    HoloInteractive holoInteractive;

    public void Start()
    {
        complete = true;
        blueprintServer = GameObject.Find("Server").GetComponent(typeof(BlueprintServer)) as BlueprintServer;
    }

    public void StartMinigame(string Instruction, string game, Vector3 Position)
    {
        if (complete)
        {
            Debug.Log("New minigame starting at " + Position + " of game type " + game);
            currentInstruction = Instruction;
            currentGame = game;
            complete = false;
            this.transform.position = Position;
        }
        else
        {
            Debug.Log("Attempting to start but game has already been started");
        }
    }

    public void Update()
    {
        if (!complete)
        {
            Camera mainCamera = CameraCache.Main;
            if (Vector3.Distance(this.transform.position, mainCamera.transform.position) < 3.0f)
            {
                blueprintServer.HideAreaHighlight();
                if (currentGame == "wood")
                {
                    if (spawned == false)
                    {
                        Debug.Log("Spawning in object for minigame");
                        gObject = Instantiate(Resources.Load("tree", typeof(GameObject))) as GameObject;
                        gObject.transform.position = this.transform.position + new Vector3(2.0f, 0.0f, 0.0f);
                        holoInteractive = gObject.AddComponent<HoloInteractive>() as HoloInteractive;
                        holoInteractive.SetAttributes("shrink", gObject.transform.position);
                        spawned = true;
                    }
                    else
                    {
                        if (holoInteractive.hidden)
                        {
                            Debug.Log("Game complete for " + currentGame);
                            complete = true;
                            Destroy(gObject);
                        }
                    }
                }
            }
        }
    }
}
