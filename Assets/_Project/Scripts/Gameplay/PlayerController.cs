using GameLogic.Core.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExtinctionMarine.Gameplay
{
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;

        private PlayerEntity logicData;
        private Rigidbody2D rb;
        private Vector2 moveInput;


        public float CurrentHp => logicData.CurrentHealth;
        public float MaxHp => logicData.MaxHealth;
        public bool IsDead => logicData.IsDead;

        private void Awake()
        {
            
            rb = GetComponent<Rigidbody2D>();
            logicData = new PlayerEntity();
        }

        public void OnMove(InputValue value)
        {
            if(IsDead) { return; }
            moveInput = value.Get<Vector2>();
        }

        private void FixedUpdate()
        {
            if (IsDead)
            {
                rb.linearVelocity = Vector2.zero;
            }
            MovePlayer();
        }

        private void MovePlayer()
        {
           
            Vector2 movement = moveInput.normalized * moveSpeed;

            rb.linearVelocity = movement;
        }

        public void ApplyDamage(float damageAmount)
        {
            if (logicData == null) { return; }
            Debug.Log($"[Unity View] -HIT! HP before: {logicData.CurrentHealth}");
            logicData.TakeDamage(damageAmount);
            Debug.Log($"[Unity View] HP after: {logicData.CurrentHealth}");
            if (logicData.IsDead)
            {
                HandleDeath();
            }

        }
        private void HandleDeath()
        {
            Debug.LogWarning("[Unity View] TRIGGER GAME OVER: Marine has been killed");
           
        }

    }
} 