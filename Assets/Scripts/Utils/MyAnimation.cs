using System.Collections;
using UnityEngine;

namespace Utils
{
    public enum Anims
    {
        moveAccelerate,
        moveDecelerate,
        oscillate
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
            int direction = 1;
            float currentPosition = 0.0f;

            for (int count = 0; ; count++)
            {
                yield return new WaitForSeconds(0.01f);
                
                float positionModifier = (direction * maxDifference - currentPosition) / 100;
                
                currentPosition += positionModifier;

                gameObject.transform.position += new Vector3(0.0f, positionModifier, 0.0f);

                if (count % 100 == 0)
                {
                    direction *= -1;
                }
            }
        }
    }
}

