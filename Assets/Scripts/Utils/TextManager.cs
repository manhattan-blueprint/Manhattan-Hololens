using UnityEngine;
using System.Collections;


namespace Utils
{
    /// <summary>
    /// Controls the informative text shown in the game and prevents contradictory overlap.
    /// </summary>
    public class TextManager : MonoBehaviour
    {
        public TextMesh infoText;
        private bool inUse;
        private int timeLeft;
        private IEnumerator timerCoroutine;

        /// <summary>
        /// Automatically called when the Unity scene is made, as described by MonoBehaviour.
        /// </summary>
        public void Start()
        {
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
            RequestReset(duration);
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

        /// <summary>
        /// Gets the current time left on the timer.
        /// </summary>
        /// <returns></returns>
        public int GetTimeLeft()
        {
            return timeLeft;
        }

        /// <summary>
        /// Starts a new timer.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="delay"></param>
        public void RequestTimer(int time, float delay)
        {
            timeLeft = time;
            timerCoroutine = DecreaseTime(1, 1, delay);
            StartCoroutine(timerCoroutine);
        }

        /// <summary>
        /// Stops a currently running timer.
        /// </summary>
        public void RequestTimerStop()
        {
            StopCoroutine(timerCoroutine);
        }

        /// <summary>
        /// Coroutine for updating the timer.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timestep"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        private IEnumerator DecreaseTime(int time, int timestep = 1, float delay = 2.0f)
        {
            yield return new WaitForSeconds(delay);

            for (; timeLeft >= 0; timeLeft -= timestep)
            {
                yield return new WaitForSeconds(timestep);
                
                infoText.text = timeLeft.ToString();
            }
        }
    }
}