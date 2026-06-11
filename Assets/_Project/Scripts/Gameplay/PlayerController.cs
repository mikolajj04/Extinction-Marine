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

        [Header("Combat Dependencies")]
        [SerializeField] private ProjectilePool projectilePool;


        private PlayerEntity logicData;
        private Rigidbody2D rb;
        private Vector2 moveInput;
        private Camera mainCamera;

        public float CurrentHp => logicData.CurrentHealth;
        public float MaxHp => logicData.MaxHealth;
        public bool IsDead => logicData.IsDead;

        private void Awake()
        {
            
            rb = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
            logicData = new PlayerEntity();
        }

        public void OnMove(InputValue value)
        {
            if(IsDead) { return; }
            moveInput = value.Get<Vector2>();
        }
        public void OnFire(InputValue value)
        {
            if (IsDead || projectilePool == null) return;

          
            if (value.isPressed)
            {
                Shoot();
            }
        }
        private void Shoot()
        {
           
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

           
            Vector2 shootDirection = (mouseWorldPos - transform.position).normalized;

            projectilePool.FireProjectile(transform.position, shootDirection);
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