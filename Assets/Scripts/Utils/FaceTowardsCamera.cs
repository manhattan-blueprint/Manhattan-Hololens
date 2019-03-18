using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Makes an object billboard (constant facing towards a camera).
    /// </summary>
    public class FaceTowardsCamera : MonoBehaviour
    {
        private Camera targetCamera;

        void Awake()
        {
            targetCamera = Camera.main;
        }
        
        void LateUpdate()
        {
            gameObject.transform.LookAt(targetCamera.transform.position);
        }
    }
}
