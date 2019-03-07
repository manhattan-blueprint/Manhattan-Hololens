using System.Collections;
using UnityEngine;

namespace Utils
{
    public class GestureInfoManager : MonoBehaviour
    {
        private SpriteRenderer gestureReady;
        private SpriteRenderer gestureHold;
        private IEnumerator timerCoroutine;

        /// <summary>
        /// Automatically called when the Unity scene is made, as described by MonoBehaviour.
        /// </summary>
        void Start()
        {
            gestureReady = GameObject.Find("GestureReady").GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            gestureHold = GameObject.Find("GestureHold").GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
            Hide();
        }

        /// <summary>
        /// Automatically called every scene update by Unity, as described by MonoBehaviour.
        /// </summary>
        void Update()
        {

        }

        public void RequestShowTapInfo()
        {
            timerCoroutine = TapInfoAnimation();
            StartCoroutine(timerCoroutine);
        }

        public void RequestShowDragInfo()
        {
            timerCoroutine = TapDragInfo();
            StartCoroutine(timerCoroutine);
        }

        public void RequestHide()
        {
            StopCoroutine(timerCoroutine);
            Hide();
        }

        private IEnumerator TapDragInfo()
        {
            gestureReady.enabled = false;
            gestureHold.enabled = true;
            for (int i = 4; i < 1000 ; i ++ )
            {
                if ((i % 5 == 0) || ((i + 1) % 5 == 0))
                {
                    SwapGestures();
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator TapInfoAnimation()
        {
            for ( ; ; )
            {
                SwapGestures();
                yield return new WaitForSeconds(0.5f);
            }
        }

        private void SwapGestures()
        {
            if (gestureReady.enabled == true)
            {
                gestureReady.enabled = false;
                gestureHold.enabled = true;
            }
            else
            {
                gestureReady.enabled = true;
                gestureHold.enabled = false;
            }
        }

        private void Hide()
        {
            gestureReady.enabled = false;
            gestureHold.enabled = false;
        }
    }
}
