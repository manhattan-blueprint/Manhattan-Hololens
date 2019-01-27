using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

/// <summary>
/// Very simple class that implements basic logic for a trigger button.
/// </summary>
public class HoloInteractive : MonoBehaviour, IInputHandler
{
    /// <summary>
    /// Indicates whether the button is clickable or not.
    /// </summary>
    [Tooltip("Indicates whether the button is clickable or not.")]
    public bool IsEnabled = true;

    public event Action ButtonPressed;

    public TextMesh infoText;

    public GameObject gObject;
    public HoloObject holoObject;

    private string[] infoTexts = {"Hello Mr Blueprint!", "Tap to collect", "Follow the pointer", ""};
    private int infoTextCounter = 0;

    public void Start()
    {
        // Make objects respond to being tapped
        // Debug.Log("Start called in holointeractive.");
        holoObject = new HoloObject(gObject, "wood");

        // Display info text
        ResetAll();
    }

    public void SpawnObject(Vector3 position)
    {
        holoObject.reset();
        gObject.transform.position = position;
    }

    public void ResetAll()
    {
        Debug.Log("Holointeractive object data reset");
        SpawnObject(new Vector3(0.0f, 0.0f, 3.5f));
        infoText.transform.position = new Vector3(0.0f, 0.3f, 3.5f);
        infoText.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        infoTextCounter = 0;
        infoText.text = infoTexts[infoTextCounter];
        Invoke("UpdateInfoText", 3);
        Invoke("UpdateInfoText", 6);
        Invoke("UpdateInfoText", 9);
    }

    public void UpdateInfoText()
    {
        infoTextCounter++;
        infoText.text = infoTexts[infoTextCounter];
    }

    public void BlankText()
    {
        infoText.text = "";
    }

    void IInputHandler.OnInputDown(InputEventData eventData)
    {
        // Intentionally left blank; required for constructor.
    }

    void IInputHandler.OnInputUp(InputEventData eventData)
    {
        if (IsEnabled && eventData.PressType == InteractionSourcePressInfo.Select)
        {
            holoObject.doGather();
        }
    }

    public void Update()
    {
        if (holoObject.getHarvestState() == true)
        {
            double textOrientation = 0.0f;
            infoText.transform.rotation = Quaternion.Euler(0.0f, (float)(-Mathf.Rad2Deg*textOrientation), 0.0f);
            infoText.text = "You collected wood. Well done!";
            Invoke("BlankText", 3);
            infoText.transform.position = gObject.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ResetAll();
            holoObject.reset();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SpawnObject(new Vector3(UnityEngine.Random.Range(-4.0f, 4.0f), 0.0f, UnityEngine.Random.Range(-4.0f, 4.0f)));
        }
    }

}
