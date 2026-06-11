using UnityEngine;
using System;

namespace ExtinctionMarine.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileController : MonoBehaviour
    {
        [SerializeField] private float speed = 12f;
        [SerializeField] private float lifeTime = 2f;

        private Rigidbody2D rb;
        private float currentLifeTime;

        private Action<ProjectileController> onDeactivate;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Vector2 position, Vector2 direction, Action<ProjectileController> deactivateCallback)
        {
        
            gameObject.SetActive(true);

           
            transform.position = position;
            onDeactivate = deactivateCallback;
            currentLifeTime = 0f;

           
            if (rb == null)
            {
                rb = GetComponent<Rigidbody2D>();
            }

           
            if (rb != null)
            {
                rb.linearVelocity = direction.normalized * speed;
            }
            else
            {
                Debug.LogError("[ProjectileController] no Rigidbody2D!");
            }
        }

        private void Update()
        {
            currentLifeTime += Time.deltaTime;
            if (currentLifeTime >= lifeTime)
            {
                Deactivate();
            }
        }

        private void Deactivate()
        {
            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);

            onDeactivate?.Invoke(this);
        }

        
        private void OnTriggerEnter2D(Collider2D other)
        {
           
            if (other.TryGetComponent<EnemyController>(out var enemy))
            {
                
                enemy.TakeDamage(5f);

               
                Deactivate();
            }
        }
    }
}