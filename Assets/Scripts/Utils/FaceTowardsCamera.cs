namespace Utils
{
    using UnityEngine;

    public class FaceTowardsCamera : MonoBehaviour
    {

        public Camera camera;
        GameObject myContainer;

        void Awake()
        {
            camera = Camera.main;
            //myContainer = new GameObject();
            //myContainer.name = "Holder_" + transform.gameObject.name;
            //myContainer.transform.position = transform.position;
            //transform.parent = myContainer.transform;
        }
        
        void LateUpdate()
        {
            gameObject.transform.LookAt(camera.transform.position);
        }
    }
}
