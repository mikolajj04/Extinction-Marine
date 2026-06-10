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
            transform.position = position;
            rb.linearVelocity = direction.normalized * speed;
            onDeactivate = deactivateCallback;
            currentLifeTime = 0f;

            gameObject.SetActive(true);
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
    }
}