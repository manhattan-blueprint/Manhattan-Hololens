using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloInteractive : MonoBehaviour {

    bool rayCasted = false;

    public string objectName;

	// Use this for initialization
	void Start () {
        Debug.Log(objectName + " ray casting initialized");
	}

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out hitInfo,
                20.0f,
                Physics.DefaultRaycastLayers))
        {
            // If the Raycast has succeeded and hit a hologram
            // hitInfo's point represents the position being gazed at
            // hitInfo's collider GameObject represents the hologram being gazed at

            if (!rayCasted)
            {
                Debug.Log(objectName + " is being looked at now!");
                rayCasted = true;
            }
        }
        else
        {
            if (rayCasted)
            {
                Debug.Log(objectName + " is not being looked at anymore");
                rayCasted = false;
            }
        }
    }
}
