using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Forces an item to be placed directly in front of the camera when attached.
    /// </summary>
    public class TrackCamera : MonoBehaviour
    {
        [Tooltip("The camera for the object this script is attached to track.")]
        public GameObject mainCamera;

        [Tooltip("The position out of the central point of view modifier")]
        public Vector3 posModifier;

        /// <summary>
        /// Automatically called when the Unity scene is made, as described by MonoBehaviour.
        /// </summary>
        void Start()
        {

        }

        /// <summary>
        /// Automatically called every scene update by Unity, as described by MonoBehaviour.
        /// </summary>
        void Update()
        {
            this.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 3.0f + posModifier;
            this.transform.rotation = mainCamera.transform.rotation;
        }
    }
}
