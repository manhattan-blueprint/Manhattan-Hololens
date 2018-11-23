using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;

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

    public GameObject wood;
    public HoloObject h_wood;

    public GameObject ore;
    public HoloObject h_ore;

    public void Start()
    {
        Debug.Log("Start called in holointeractive.");
        h_wood = new HoloObject(wood, "wood");
        h_ore = new HoloObject(ore, "ore");
    }

    /// <summary>
    /// Press the button programmatically.
    /// </summary>
    public void Press()
    {
        if (IsEnabled)
        {
        }
    }

    void IInputHandler.OnInputDown(InputEventData eventData)
    {
        // Nothing.
    }

    void IInputHandler.OnInputUp(InputEventData eventData)
    {
        if (IsEnabled && eventData.PressType == InteractionSourcePressInfo.Select)
        {
            h_wood.doGather();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            h_wood.reset();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            h_ore.reset();
        }
    }
}

