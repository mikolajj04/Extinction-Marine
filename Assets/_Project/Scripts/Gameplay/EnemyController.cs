using System;
using UnityEngine;
using GameLogic.Core.Models;


namespace ExtinctionMarine.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Combat Settings")]
        [SerializeField] private float biteDamage = 15f;
        [SerializeField] private float attackCooldown = 1f;
        public static event Action OnEnemyKilled;
        private Transform playerTransform;
        private RaptorEntity logicData;
        private Rigidbody2D rb;
        private Action<EnemyController> returnToPool;
        private float nextAttackTime = 0f;

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
            nextAttackTime = 0f;
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


        private void OnCollisionStay2D(Collision2D collision)
        {
            
            if (logicData == null || logicData.IsDead) return;

           
            if (Time.time >= nextAttackTime)
            {
                if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
                {
                    player.ApplyDamage(biteDamage);

                   
                    nextAttackTime = Time.time + attackCooldown;

                    Debug.Log($"[EnemyController] Raptor bites the player for {biteDamage} damage!");
                }
            }
        }
    }
}
