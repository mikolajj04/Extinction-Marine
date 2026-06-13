using System;
using UnityEngine;
using GameLogic.Core.Models;


namespace ExtinctionMarine.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        public static event Action OnEnemyKilled;
        private Transform playerTransform;
        private RaptorEntity logicData;
        private Rigidbody2D rb;
        private Action<EnemyController> returnToPool;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Transform target, Action<EnemyController> onDeathCallback)
        {
            playerTransform = target;
            returnToPool = onDeathCallback;

            logicData = new RaptorEntity();

            rb.linearVelocity = Vector2.zero;
            gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            if (logicData == null || logicData.IsDead || playerTransform == null)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 direction = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = direction * logicData.Speed;
        }

        public void TakeDamage(float amount)
        {
            if (logicData == null || logicData.IsDead) return;

            logicData.TakeDamage(amount);
            Debug.Log($"[EnemyController] Raptor took {amount} damage. HP: {logicData.CurrentHealth}");

            if (logicData.IsDead)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("[EnemyController] Raptor eliminated, recycling into pool.");
            OnEnemyKilled?.Invoke();
            returnToPool?.Invoke(this);
        }

       
        private void OnCollisionEnter2D(Collision2D collision)
        {
          
            if (logicData == null || logicData.IsDead) return;

            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            {
                player.ApplyDamage(15f);
                Debug.Log("[EnemyController] Raptor bites the player for 15 damage!");
            }
        }
    }
}