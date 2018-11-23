using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloObject {

    public GameObject gameObject;

    private bool isVisible;
    private float shrinkAmount;
    private string objType;
    private Vector3 originalScale;

    public HoloObject (GameObject inpObject, string inpObjType)
    {
        isVisible = true;
        gameObject = inpObject;
        objType = inpObjType;
        originalScale = inpObject.transform.localScale;
        shrinkAmount = gameObject.transform.localScale.x / 4;
    }

    public void reset()
    {
        isVisible = true;
        gameObject.transform.localScale = originalScale;
    }
    
    public void hide ()
    {
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void doGather ()
    {
        if (isVisible)
        {
            if (objType == "wood") {
                if (gameObject.transform.localScale.x >= shrinkAmount)
                {
                    gameObject.transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);
                }
                else
                {
                    hide();
                }
            }
        }
    }
}
