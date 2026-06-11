using UnityEngine;
using GameLogic.Core.Models;

namespace ExtinctionMarine.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Targeting")]
        [SerializeField] private Transform playerTransform;


        private RaptorEntity logicData;
        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            logicData = new RaptorEntity();
        }

        private void FixedUpdate()
        {

            if (logicData.IsDead || playerTransform == null)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            ChasePlayer();
        }

        private void ChasePlayer()
        {

            Vector2 direction = (playerTransform.position - transform.position).normalized;


            rb.linearVelocity = direction * logicData.Speed;
        }


        public void TakeDamage(float amount)
        {
            if (logicData.IsDead) return;


            logicData.TakeDamage(amount);

            Debug.Log($"[EnemyController] Raptor took {amount} damage! Remaining HP: {logicData.CurrentHealth}");

            if (logicData.IsDead)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("[EnemyController] Target eliminated! Raptor is DEAD!");

            Destroy(gameObject);
        }

     
        private void OnCollisionEnter2D(Collision2D collision)
        {
          
            if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
            {
              
                player.ApplyDamage(15f);

                Debug.Log("[EnemyController] Raptor bites the player for 15 damage!");
            }
        }
    }
}