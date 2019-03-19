using System.Collections;
using UnityEngine;

namespace Utils
{
    public enum Anims
    {
        moveAccelerate,
        moveDecelerate,
        oscillate,
        grow
    }

    class MyAnimation : MonoBehaviour
    {
        private IEnumerator timedCoroutine;

        public void Start()
        {

        }

        public void Update()
        {

        }

        public void StartAnimation(Anims animation, Vector3 moveVector, float speed = 0.001f, bool destroyAfter = false, float delay = 0.0f, float modifier = 200.0f)
        {
            switch (animation)
            {
                case Anims.moveAccelerate:
                    timedCoroutine = MoveToAccelerate(moveVector, 0.1f, destroyAfter, delay, modifier);
                    break;

                case Anims.oscillate:
                    timedCoroutine = Oscillate();
                    break;

                case Anims.grow:
                    timedCoroutine = Grow(speed, delay);
                    break;

                default:
                    timedCoroutine = MoveToAccelerate(moveVector, 0.1f, destroyAfter, delay, modifier);
                    break;
            }

            StartCoroutine(timedCoroutine);
        }

        private IEnumerator MoveToAccelerate(Vector3 moveVector, float speed, bool destroyAfter, float delay, float maxHeight)
        {
            yield return new WaitForSeconds(delay);

            for (int count = 0; count <= 1000; count ++)
            {
                yield return new WaitForSeconds(0.01f);
                gameObject.transform.position += (moveVector / 1000 * count) / 250;
            }

            if (destroyAfter)
                MonoBehaviour.Destroy(gameObject);
        }

        private IEnumerator MoveToDecelerate(Vector3 moveVector, float speed, bool destroyAfter, float delay, float maxHeight)
        {
            yield return new WaitForSeconds(delay);
        }

        private IEnumerator Oscillate()
        {
            float maxDifference = 0.5f;

            for (int count = 0; ; count++)
            {
                yield return new WaitForSeconds(0.01f);

                float positionModifier = maxDifference * Mathf.Sin((Mathf.PI * count) / 90) / 200;

                gameObject.transform.position += new Vector3(0.0f, positionModifier, 0.0f);
            }
        }

        private IEnumerator Grow(float targetSize, float delay)
        {
            gameObject.transform.localScale = Vector3.zero;

            yield return new WaitForSeconds(delay);

            for (float count = 0.0f; count < 110.0f; count += 1.5f)
            {
                yield return new WaitForSeconds(0.01f);

                // Overshoots and then shrinks back a little bit
                float size = targetSize * 1.05f * Mathf.Sin((Mathf.PI * count) / 180);

                gameObject.transform.localScale = new Vector3(size, size, size);
            }
        }
    }
}

