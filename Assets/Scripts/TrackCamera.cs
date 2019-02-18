/*
Makes an object track in front of the camera, similar to how a UI appears
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCamera : MonoBehaviour
{
    public GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        float projectionDistance = 3.0f;
        Vector3 distFrom = new Vector3(0.0f, 0.0f, projectionDistance);
        
        this.transform.position = cameraPosition + distFrom;
    }
}
