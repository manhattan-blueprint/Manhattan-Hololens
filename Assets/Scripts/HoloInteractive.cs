using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public enum InteractType {Drag, Rotate, ClickShrink};
public enum InteractState {Idle, Touched, Hidden};

public class HoloInteractive : MonoBehaviour, IFocusable, IInputClickHandler, IManipulationHandler, INavigationHandler
{
    readonly float rotSensitivity = 10.0f;
    readonly float dragSensitivity = 1.0f;
    
    public InteractType interactType;
    public InteractState interactState;
    
    private Vector3 originalScale;
    private float shrinkAmount;
    private Vector3 manipulationOriginalPosition = Vector3.zero;

    public void Start()
    {
        interactState = InteractState.Idle;
        originalScale = this.transform.localScale;
    }

    public void Update()
    {

    }

    public void SetAttributes(InteractType interactType, int divs = 8)
    {
        this.interactType = interactType;
        shrinkAmount = this.transform.localScale.y / (divs + 1);
    }

    public void Hide ()
    {
        this.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        interactState = InteractState.Hidden;
    }

    public void OnFocusEnter()
    {

    }

    public void OnFocusExit() 
    {

    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        interactState = InteractState.Touched;
        if (interactType == InteractType.ClickShrink)
        {
            if (this.transform.localScale.y >= shrinkAmount * 1.1f)
            {
                this.transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);
                this.transform.position += new Vector3(0.0f, -shrinkAmount * 3.0f, 0.0f);
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
        interactState = InteractState.Touched;
        if (this.interactType == InteractType.Drag)
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
        interactState = InteractState.Touched;
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
