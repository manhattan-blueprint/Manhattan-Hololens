using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System.Collections;

public enum InteractType {Drag, Rotate, ClickShrink};
public enum InteractState {Idle, Touched, Hidden};

namespace Utils
{
    /// <summary>
    /// Adds movement capabilities to objects such as dragging, clicking and rotating.
    /// </summary>
    public class HoloInteractive : MonoBehaviour, IFocusable, IInputClickHandler, IManipulationHandler, INavigationHandler
    {
        readonly float rotSensitivity = 10.0f;
#if NETFX_CORE
        readonly float dragSensitivity = 3.0f;
#else
        readonly float dragSensitivity = 5.0f;
#endif

        [Tooltip("Sets the way an object is interacted with the options available being defined in InteractType.")]
        public InteractType interactType;

        [Tooltip("The current state of the object being manipulated, important for determining whether to start, update or complete.")]
        public InteractState interactState;

        private Vector3 originalScale;
        private float shrinkAmount;
        private Vector3 manipulationOriginalPosition;
        private bool gravity;

        /// <summary>
        /// Automatically called when the Unity scene is made, as described by MonoBehaviour.
        /// </summary>
        public void Start()
        {
            interactState = InteractState.Idle;
            manipulationOriginalPosition = Vector3.zero;
        }

        /// <summary>
        /// Automatically called every scene update by Unity, as described by MonoBehaviour.
        /// </summary>
        public void Update()
        {

        }

        /// <summary>
        /// Sets the way objects interact if this script is attached.
        /// </summary>
        /// <param name="interactType"></param>
        /// <param name="divs"></param>
        public void SetAttributes(InteractType interactType, int divs = 8, bool gravity = false, float originalScale = 10.0f)
        {
            this.originalScale = new Vector3(originalScale, originalScale, originalScale);
            this.interactType = interactType;
            shrinkAmount = originalScale / (float)(divs);
            this.gravity = gravity;
            Debug.Log("Holointeractive object made with shrink amount " + shrinkAmount);
            Debug.Log("Local scale is  " + this.transform.localScale);
        }

        /// <summary>
        /// Hides the object.
        /// </summary>
        public void Hide()
        {
            this.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            interactState = InteractState.Hidden;
        }

        /// <summary>
        /// Effect when object is looked at, as described by IFocusable interface.
        /// </summary>
        public void OnFocusEnter()
        {

        }

        /// <summary>
        /// Effect when object is looked away from after being looked at, as described by IFocusable interface.
        /// </summary>
        public void OnFocusExit()
        {

        }

        /// <summary>
        /// Effect when an object is clicked, as described by IInputClickHandler interace.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnInputClicked(InputClickedEventData eventData)
        {
            Debug.Log("Original scale is  " + originalScale.y);
            Debug.Log("Object clicked");
            Debug.Log("Local scale: " + this.transform.localScale.y);
            Debug.Log("Limit: " + ((float)originalScale.y / 10.0f));
            interactState = InteractState.Touched;
            if (interactType == InteractType.ClickShrink)
            {
                if (this.transform.localScale.y >= shrinkAmount * 1.1f )
                {
                    this.transform.localScale -= new Vector3(shrinkAmount, shrinkAmount, shrinkAmount);
                    //this.transform.position += new Vector3(0.0f, -shrinkAmount * 0.3f, 0.0f);
                }
                else
                {
                    Hide();
                }
            }
        }

        /// <summary>
        /// Effect when an object is initially tapped and held to translate the object, as described by IManipulationHandler interface.
        /// </summary>
        /// <param name="eventData"></param>
        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
        {
            interactState = InteractState.Touched;
            if (this.interactType == InteractType.Drag)
            {
                if (gravity)
                {
                    gameObject.GetComponent<Rigidbody>().useGravity = false;
                }
                InputManager.Instance.PushModalInputHandler(gameObject);
                manipulationOriginalPosition = transform.position;
            }
        }

        /// <summary>
        /// Effect when an object remains held to translate the object, as described by IManipulationHandler interface.
        /// </summary>
        /// <param name="eventData"></param>
        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
        {
            if (interactType == InteractType.Drag)
            {
                transform.position = manipulationOriginalPosition + eventData.CumulativeDelta * dragSensitivity;
            }
        }

        /// <summary>
        /// Effect when an object is released after being held to translate, as described by IManipulationHandler interface.
        /// </summary>
        /// <param name="eventData"></param>
        void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
        {
            if (gravity)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
            interactState = InteractState.Idle;
            if (interactType == InteractType.Drag)
            {
                InputManager.Instance.PopModalInputHandler();
            }
        }

        /// <summary>
        /// Effect when an object is tapped and then the hold cancelled due to an external event, as described by IManipulationHandler interface.
        /// </summary>
        /// <param name="eventData"></param>
        void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
        {
            if (gravity)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
            interactState = InteractState.Idle;
            if (interactType == InteractType.Drag)
            {
                InputManager.Instance.PopModalInputHandler();
            }
        }

        /// <summary>
        /// Effect when an object is initially tapped and held to rotate the object, as described by INavigationHandler interface.
        /// </summary>
        /// <param name="eventData"></param>
        void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
        {
            interactState = InteractState.Touched;
            if (interactType == InteractType.Rotate)
            {
                InputManager.Instance.PushModalInputHandler(gameObject);
            }
        }

        /// <summary>
        /// Effect when an object has already been tapped and held to rotate the object, as described by INavigationHandler interface.
        /// </summary>
        /// <param name="eventData"></param>
        void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
        {
            if (interactType == InteractType.Rotate)
            {
                float rotationFactor = eventData.NormalizedOffset.x * rotSensitivity;
                transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
            }
        }

        /// <summary>
        /// Effect when an object is released when being held to rotate the object, as described by INavigationHandler interface.
        /// </summary>
        /// <param name="eventData"></param>
        void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
        {
            if (interactType == InteractType.Rotate)
            {
                InputManager.Instance.PopModalInputHandler();
            }
        }

        /// <summary>
        /// Effect when an object is tapped and held to rotate the object but then the interaction is cancelled, as described by INavigationHandler interface.
        /// </summary>
        /// <param name="eventData"></param>
        void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
        {
            if (interactType == InteractType.Rotate)
            {
                InputManager.Instance.PopModalInputHandler();
            }
        }

    }
}
