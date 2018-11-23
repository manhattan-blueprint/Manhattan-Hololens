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

    public GameObject p_collectable;

    /// <summary>
    /// Press the button programmatically.
    /// </summary>
    public void Press()
    {
        if (IsEnabled)
        {
            p_collectable.transform.localScale -= new Vector3(0.2f, 0, 0);
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
            p_collectable.transform.localScale -= new Vector3(0.2f, 0, 0);
            eventData.Use(); // Mark the event as used, so it doesn't fall through to other handlers.
        }
    }
}
