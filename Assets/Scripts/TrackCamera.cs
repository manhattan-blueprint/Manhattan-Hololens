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
        this.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 3.0f;
        this.transform.rotation = mainCamera.transform.rotation;
    }
}
