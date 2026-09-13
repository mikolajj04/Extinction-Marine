using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ExtinctionMarine.Gameplay.UI
{
    [RequireComponent(typeof(Image))]
    public class LampFlicker : MonoBehaviour
    {
        [Header("Flicker Timing")]
        [Tooltip("Minimum stable illumination time (in seconds)")]
        [SerializeField] private float minInterval = 4f;

        [Tooltip("Maximum stable illumination time (in seconds)")]
        [SerializeField] private float maxInterval = 9f;

        [Header("Visual Intensity")]
        [Tooltip("Maximum shadow transparency (0.3 = 30% opacity)")]
        [Range(0f, 1f)]
        [SerializeField] private float maxShadowAlpha = 0.3f;

        private Image shadowImage;

        private void Awake()
        {
            shadowImage = GetComponent<Image>();
            SetAlpha(0f);
        }

        private void Start()
        {
            StartCoroutine(FlickerRoutine());
        }

        private IEnumerator FlickerRoutine()
        {
            while (true)
            {
                float waitTime = Random.Range(minInterval, maxInterval);
                yield return new WaitForSecondsRealtime(waitTime);

                int flickersCount = Random.Range(2, 6);

                for (int i = 0; i < flickersCount; i++)
                {
                    float currentFlickerIntensity = Random.Range(maxShadowAlpha * 0.5f, maxShadowAlpha);
                    SetAlpha(currentFlickerIntensity);
                    yield return new WaitForSecondsRealtime(Random.Range(0.04f, 0.12f));

                    SetAlpha(0f);
                    yield return new WaitForSecondsRealtime(Random.Range(0.05f, 0.15f));
                }
            }
        }

        private void SetAlpha(float alphaTarget)
        {
            if (shadowImage != null)
            {
                Color c = shadowImage.color;
                c.a = alphaTarget;
                shadowImage.color = c;
            }
        }
    }
}