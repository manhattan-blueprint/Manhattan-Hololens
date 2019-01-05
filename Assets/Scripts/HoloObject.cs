using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloObject {

    public GameObject gameObject;

    private bool isVisible;
    private float shrinkAmount;
    private string objType;
    private Vector3 originalScale;
    private bool harvested = false;

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
        harvested = false;
        gameObject.transform.localScale = originalScale;
    }

    public void hide ()
    {
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        isVisible = false;
        harvested = true;
    }

    public void doGather ()
    {
        if (isVisible)
        {
            if (objType == "wood") {
                if (gameObject.transform.localScale.x >= shrinkAmount * 1.1f)
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

    // Returns harvest state. If harvest state is true then it sets it to false, so this function
    // will only return true once for each object harvested!
    public bool getHarvestState()
    {
        if (harvested)
        {
            harvested = false;
            return true;
        }
        return false;
    }
}
