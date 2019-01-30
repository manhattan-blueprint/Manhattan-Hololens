using System;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class HoloInteractive : MonoBehaviour, IFocusable, IInputClickHandler
{
    private string objType;
    private Vector3 originalScale;
    private float shrinkAmount;

    public void Start()
    {
        Debug.Log("HoloInteractive: instantiated");
        originalScale = this.transform.localScale;
        shrinkAmount = this.transform.localScale.x / 8;
    }

    public void SetAttributes(String objType, Vector3 position)
    {
        this.transform.position = position;
        this.objType = objType;
    }

    public void Hide ()
    {
        this.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void ResetSize()
    {
        this.transform.localScale = originalScale;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ResetSize();
        }
    }

    public void OnFocusEnter()
    {

    }

    public void OnFocusExit() 
    {

    }

    public void OnInputClicked(InputClickedEventData eventData) 
    {
        Debug.Log("HoloInteractive: clicked");
        if (objType == "wood") {
        }
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
