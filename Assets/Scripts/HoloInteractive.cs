using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public enum InteractType {Drag, Rotate, ClickShrink};

public class HoloInteractive : MonoBehaviour, IFocusable, IInputClickHandler, IManipulationHandler, INavigationHandler
{
    readonly float rotSensitivity = 10.0f;
    readonly float dragSensitivity = 3.0f;

    public bool hidden;
    public InteractType interactType;
    
    private Vector3 originalScale;
    private float shrinkAmount;
    private Vector3 manipulationOriginalPosition = Vector3.zero;

    public void Start()
    {
        originalScale = this.transform.localScale;
        shrinkAmount = this.transform.localScale.x / 8;
        hidden = false;
    }

    public void Update()
    {

    }

    public void SetAttributes(InteractType interactType)
    {
        this.interactType = interactType;
    }

    public void Hide ()
    {
        this.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        hidden = true;
    }

    public void OnFocusEnter()
    {

    }

    public void OnFocusExit() 
    {

    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        if (interactType == InteractType.ClickShrink)
        {
            if (this.transform.localScale.x >= shrinkAmount * 1.1f)
            {
                this.transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);
            }
            else
            {
                Hide();
            }
        }
    }


    // Click and hold to drag
    void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
    {
        if (interactType == InteractType.Drag)
        {
            InputManager.Instance.PushModalInputHandler(gameObject);
            manipulationOriginalPosition = transform.position;
        }
    }

    void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (interactType == InteractType.Drag)
        {
            transform.position = manipulationOriginalPosition + eventData.CumulativeDelta * dragSensitivity;
        }
    }

    void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
    {
        if (interactType == InteractType.Drag)
        {
            InputManager.Instance.PopModalInputHandler();
        }
    }

    void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
    {
        if (interactType == InteractType.Drag)
        {
            InputManager.Instance.PopModalInputHandler();
        }
    }


    // Click and hold to rotate.
    void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
    {
        if (interactType == InteractType.Rotate)
        {
            InputManager.Instance.PushModalInputHandler(gameObject);
        }
    }

    void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
    {
        if (interactType == InteractType.Rotate)
        {
            float rotationFactor = eventData.NormalizedOffset.x * rotSensitivity;
            transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
        }
    }

    void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
    {
        if (interactType == InteractType.Rotate)
        {
            InputManager.Instance.PopModalInputHandler();
        }
    }

    void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
    {
        if (interactType == InteractType.Rotate)
        {
            InputManager.Instance.PopModalInputHandler();
        }
    }

}
