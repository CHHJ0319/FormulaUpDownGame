using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BlinkingPrompt : MonoBehaviour
    {
        private Image targetImage;
        [SerializeField] private float blinkSpeed = 1.0f;

        void Start()
        {
            targetImage = GetComponent<Image>();

            StartCoroutine(DoBlink());
        }

        IEnumerator DoBlink()
        {
            while (true)
            {
                for (float f = 0f; f <= 1f; f += Time.deltaTime * blinkSpeed)
                {
                    Color c = targetImage.color;
                    c.a = f;
                    targetImage.color = c;
                    yield return null;
                }

                for (float f = 1f; f >= 0f; f -= Time.deltaTime * blinkSpeed)
                {
                    Color c = targetImage.color;
                    c.a = f;
                    targetImage.color = c;
                    yield return null;
                }
            }
        }
    }
}
