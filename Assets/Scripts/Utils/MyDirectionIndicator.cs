// Modified from Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using HoloToolkit.Unity;

namespace Utils
{
    namespace HoloToolkit.Unity
    {
        /// <summary>
        /// Adds an indicator to point to a prefab.
        /// </summary>
        public class MyDirectionIndicator : MonoBehaviour
        {
            readonly float indicatorScale = 0.5f;
            readonly float forwardCutoff = 3.0f;

            private GameObject Cursor;

            // Default object.
            [Tooltip("Model to display the direction to the object this script is attached to.")]
            private GameObject DirectionIndicatorObject;
            private GameObject ForwardIndicatorObject;

            [Tooltip("Color to shade the direction indicator.")]
            public Color DirectionIndicatorColor;

            [Tooltip("Allowable percentage inside the holographic frame to continue to show a directional indicator.")]
            [Range(-0.3f, 0.3f)]
            public float VisibilitySafeFactor = 0.1f;

            [Tooltip("Multiplier to decrease the distance from the cursor center an object is rendered to keep it in view.")]
            [Range(0.1f, 1.0f)]
            public float MetersFromCursor = 0.3f;

            // The default rotation of the cursor direction indicator.
            private Quaternion directionIndicatorDefaultRotation = Quaternion.identity;

            // Cache the MeshRenderer for the on-cursor indicator since it will be enabled and disabled frequently.
            private Renderer directionIndicatorRenderer;

            // Cache the Material to prevent material leak.
            private Material indicatorMaterial;

            // Check if the cursor direction indicator is visible.
            private bool isDirectionIndicatorVisible;
            private bool wasDirectionIndicatorVisible;

            /// <summary>
            /// Sets the indicator object and the cursor the indicator is drawn relative to.
            /// </summary>
            /// <param name="indicator"></param>
            /// <param name="cursor"></param>
            public void SetAttributes(GameObject indicator, GameObject cursor)
            {
                this.DirectionIndicatorObject = GameObject.Find("DirectionalIndicator");
                this.Cursor = GameObject.Find("CursorWithFeedback");
            }

            /// <summary>
            /// Automatically called when the script initially begins running, as described by MonoBehaviour
            /// </summary>
            public void Awake()
            {
                // Uses in game cursor and direction indicator; both must be named right or it will not work.
                if (Cursor == null)
                {
                    this.Cursor = GameObject.Find("CursorWithFeedback");
                }

                if (DirectionIndicatorObject == null)
                {
                    this.DirectionIndicatorObject = GameObject.Find("DirectionalIndicator");
                }

                if (ForwardIndicatorObject == null)
                {
                    this.ForwardIndicatorObject = GameObject.Find("ForwardIndicator");
                }

                // Instantiate the direction indicator.
                DirectionIndicatorObject = InstantiateDirectionIndicator(DirectionIndicatorObject);

                if (DirectionIndicatorObject == null)
                {
                    Debug.LogError("Direction Indicator failed to instantiate.");
                }
            }

            /// <summary>
            /// Removed the cursor object from the Unity scene.
            /// </summary>
            public void OnDestroy()
            {
                DestroyImmediate(indicatorMaterial);
                Destroy(DirectionIndicatorObject);
            }

            /// <summary>
            /// Instantiates the object, sets material and visibility.
            /// </summary>
            /// <param name="directionIndicator"></param>
            /// <returns></returns>
            private GameObject InstantiateDirectionIndicator(GameObject directionIndicator)
            {
                if (directionIndicator == null)
                {
                    return null;
                }
                GameObject indicator = Instantiate(directionIndicator);

                // Set local variables for the indicator.
                directionIndicatorDefaultRotation = indicator.transform.rotation;
                directionIndicatorRenderer = indicator.GetComponent<Renderer>();

                // Start with the indicator disabled.
                wasDirectionIndicatorVisible = false;
                isDirectionIndicatorVisible = false;
                DirectionIndicatorObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                ForwardIndicatorObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

                // Remove any colliders and rigidbodies so the indicators do not interfere with Unity's physics system.
                foreach (Collider indicatorCollider in indicator.GetComponents<Collider>())
                {
                    Destroy(indicatorCollider);
                }

                foreach (Rigidbody rigidBody in indicator.GetComponents<Rigidbody>())
                {
                    Destroy(rigidBody);
                }

                indicatorMaterial = directionIndicatorRenderer.material;
                indicatorMaterial.color = DirectionIndicatorColor;
                indicatorMaterial.SetColor("_TintColor", DirectionIndicatorColor);

                return indicator;
            }

            /// <summary>
            /// Automatically called every scene update by Unity, as described by MonoBehaviour
            /// </summary>
            public void Update()
            {
                if (DirectionIndicatorObject == null)
                {
                    return;
                }
                if (this.transform.localScale.x <= 0.01f)
                {
                    return;
                }
                Camera mainCamera = CameraCache.Main;
                // Direction from the Main Camera to this script's parent gameObject.
                Vector3 camToObjectDirection = gameObject.transform.position - mainCamera.transform.position;
                camToObjectDirection.Normalize();

                // The cursor indicator should only be visible if the target is not visible.
                isDirectionIndicatorVisible = !IsTargetVisible(mainCamera);

                // Need to hide or show object (done here with scale rather than renderer).
                if (isDirectionIndicatorVisible && !wasDirectionIndicatorVisible)
                {
                    DirectionIndicatorObject.transform.localScale = new Vector3(indicatorScale, indicatorScale, indicatorScale);
                    ForwardIndicatorObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    wasDirectionIndicatorVisible = isDirectionIndicatorVisible;
                }

                if (!isDirectionIndicatorVisible && wasDirectionIndicatorVisible)
                {
                    DirectionIndicatorObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                    ForwardIndicatorObject.transform.localScale = new Vector3(indicatorScale, indicatorScale / 2.0f, indicatorScale);
                    wasDirectionIndicatorVisible = isDirectionIndicatorVisible;
                }

                // Don't want to show the forwards direction indicator if the player is too close.
                if (Vector3.Distance(this.transform.position, mainCamera.transform.position) < forwardCutoff)
                {
                    ForwardIndicatorObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                }

                directionIndicatorRenderer.enabled = isDirectionIndicatorVisible;

                if (isDirectionIndicatorVisible)
                {
                    Vector3 position;
                    Quaternion rotation;
                    GetDirectionIndicatorPositionAndRotation(
                        camToObjectDirection,
                        mainCamera.transform,
                        out position,
                        out rotation);

                    DirectionIndicatorObject.transform.position = position;
                    DirectionIndicatorObject.transform.rotation = rotation;
                }
            }

            /// <summary>
            /// Returns whether target is visible.
            /// </summary>
            /// <param name="mainCamera"></param>
            /// <returns></returns>
            private bool IsTargetVisible(Camera mainCamera)
            {
                // This will return true if the target's mesh is within the Main Camera's view frustums.
                Vector3 targetViewportPosition = mainCamera.WorldToViewportPoint(gameObject.transform.position);
                return (targetViewportPosition.x > VisibilitySafeFactor &&
                        targetViewportPosition.x < 1 - VisibilitySafeFactor &&
                        targetViewportPosition.y > VisibilitySafeFactor &&
                        targetViewportPosition.y < 1 - VisibilitySafeFactor &&
                        targetViewportPosition.z > 0);
            }

            /// <summary>
            /// Returns the position and rotation of the indicator.
            /// </summary>
            /// <param name="camToObjectDirection"></param>
            /// <param name="cameraTransform"></param>
            /// <param name="position"></param>
            /// <param name="rotation"></param>
            private void GetDirectionIndicatorPositionAndRotation(Vector3 camToObjectDirection,
              Transform cameraTransform, out Vector3 position, out Quaternion rotation)
            {
                // Find position:
                // Save the cursor transform position in a variable.
                Vector3 origin = Cursor.transform.position;
                // Project the camera to target direction onto the screen plane.
                Vector3 cursorIndicatorDirection = Vector3.ProjectOnPlane(camToObjectDirection, -1 * cameraTransform.forward);
                cursorIndicatorDirection.Normalize();

                // If the direction is 0, set the direction to the right.
                // This will only happen if the camera is facing directly away from the target.
                if (cursorIndicatorDirection == Vector3.zero)
                {
                    cursorIndicatorDirection = cameraTransform.right;
                }

                // The final position is translated from the center of the screen along this direction vector.
                position = origin + cursorIndicatorDirection * MetersFromCursor;

                // Find the rotation from the facing direction to the target object.
                rotation = Quaternion.LookRotation(cameraTransform.forward, cursorIndicatorDirection) * directionIndicatorDefaultRotation;
            }
        }
    }
}