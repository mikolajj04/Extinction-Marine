using UnityEngine;

namespace ExtinctionMarine.Gameplay.Controllers
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }

        [Header("Tracking Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private float smoothTime = 0.15f;

        private float shakeDuration = 0f;
        private float shakeMagnitude = 0f;
        private float dampingSpeed = 1.0f;

        private Vector3 velocity = Vector3.zero;

       
        private Vector3 basePosition;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            if (target != null)
            {
               
                basePosition = new Vector3(target.position.x, target.position.y, transform.position.z);
            }
        }

        public void TriggerShake(float duration, float magnitude)
        {
            shakeDuration = duration;
            shakeMagnitude = magnitude;
        }

        private void LateUpdate()
        {
            if (target == null) return;

         
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
 
            basePosition = Vector3.SmoothDamp(basePosition, targetPosition, ref velocity, smoothTime);
     
            Vector3 finalPosition = basePosition;

           
            if (shakeDuration > 0)
            {
                Vector3 shakeOffset = (Vector3)Random.insideUnitCircle * shakeMagnitude;
                finalPosition += shakeOffset;

                shakeDuration -= Time.deltaTime * dampingSpeed;
            }

            
            transform.position = finalPosition;
        }
    }
}