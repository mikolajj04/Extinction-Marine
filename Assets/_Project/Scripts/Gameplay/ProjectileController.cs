using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtinctionMarine.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileController : MonoBehaviour
    {
        
        [SerializeField] private float lifeTime = 2f;


        private Rigidbody2D rb;
        private float currentLifeTime;
        private float projectileDamage;
        private int remainingPierce;
        private float currentSpeed;
        private HashSet<Collider2D> piercedEnemies = new HashSet<Collider2D>();

        private Action<ProjectileController> onDeactivate;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Vector2 position, Vector2 direction, float damage, float speed, int pierceCount, Action<ProjectileController> deactivateCallback)
        {

            gameObject.SetActive(true);
            transform.position = position;

            projectileDamage = damage;
            remainingPierce = pierceCount; 
            piercedEnemies.Clear(); 

            onDeactivate = deactivateCallback;
            currentLifeTime = 0f;
            currentSpeed = speed;

            rb.linearVelocity = direction.normalized * currentSpeed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
           
                if (piercedEnemies.Contains(other)) return;

                enemy.TakeDamage(projectileDamage);
                piercedEnemies.Add(other);
                remainingPierce--;
                currentSpeed = Mathf.Max(2f, currentSpeed - 5f);
                rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;

                if (remainingPierce <= 0)
                {
                    Deactivate();
                }
            }
        }
    }
}