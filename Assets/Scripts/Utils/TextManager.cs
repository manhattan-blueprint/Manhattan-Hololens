using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Controls the informative text shown in the game and prevents contradictory overlap.
    /// </summary>
    public class TextManager : MonoBehaviour
    {
        private TextMesh infoText;
        private bool inUse;

        /// <summary>
        /// Automatically called when the Unity scene is made, as described by MonoBehaviour.
        /// </summary>
        public void Start()
        {
            infoText = GameObject.Find("InfoText").GetComponent(typeof(TextMesh)) as TextMesh;
            inUse = false;
        }

        /// <summary>
        /// Automatically called every scene update by Unity, as described by MonoBehaviour.
        /// </summary>
        public void Update()
        {

        }

        /// <summary>
        /// Requests for some text to be added to the buffer of texts to be shown.
        /// </summary>
        /// <param name="text"></param>
        public void RequestText(string text)
        {
            infoText.text = text;
            inUse = true;
        }

        /// <summary>
        /// Requests for some text to be added to the buffer of texts for a specific period of time.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="duration"></param>
        public void RequestText(string text, float duration)
        {
            RequestText(text);
            Invoke("RequestReset", duration);
        }

        /// <summary>
        /// Requests for the text to be set blank.
        /// </summary>
        public void RequestReset()
        {
            infoText.text = "";
        }

        /// <summary>
        /// Requests for the text to be set blank after a period of time.
        /// </summary>
        /// <param name="delay"></param>
        public void RequestReset(float delay)
        {
            Invoke("RequestReset", delay);
        }
    }
}