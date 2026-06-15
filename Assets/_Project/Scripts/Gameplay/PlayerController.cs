using GameLogic.Core.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using ExtinctionMarine.Gameplay.UI;

namespace ExtinctionMarine.Gameplay
{
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private HealthBar expBar; 
        private float currentMaxExp = 100f; 
        [Header("UI Dependencies")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private GameOverScreen gameOverScreen;

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;

        [Header("Combat Dependencies")]
        [SerializeField] private ProjectilePool projectilePool;
        [SerializeField] private float fireRate = 0.2f;

        private PlayerEntity logicData;
        private Rigidbody2D rb;
        private Vector2 moveInput;
        private Camera mainCamera;

        private bool isFiring;
        private float fireCooldownTimer;

        public float CurrentHp => logicData.CurrentHealth;
        public float MaxHp => logicData.MaxHealth;
        public bool IsDead => logicData.IsDead;


        private void Awake()
        {
            
            rb = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
            logicData = new PlayerEntity();
        }

        private void Start()
        {
            if (healthBar != null)
            {
                healthBar.UpdateBar(logicData.CurrentHealth, logicData.MaxHealth);
            }

            if (expBar != null)
            {
                
                currentMaxExp = logicData.Level * 100f;

               
                expBar.UpdateBar(logicData.Experience, currentMaxExp);
            }
        }

        public void OnMove(InputValue value)
        {
            if(IsDead) { return; }
            moveInput = value.Get<Vector2>();
        }
        public void OnFire(InputValue value)
        {
            if (IsDead || projectilePool == null) return;


            isFiring = value.isPressed;
        }
        private void Update()
        {
            if (IsDead) return;

  
            fireCooldownTimer -= Time.deltaTime;

       
            if (isFiring && fireCooldownTimer <= 0f)
            {
                Shoot();
                fireCooldownTimer = fireRate;
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
            if (healthBar != null)
            {
                healthBar.UpdateBar(logicData.CurrentHealth, logicData.MaxHealth);
            }
            if (logicData.IsDead)
            {
                HandleDeath();
            }

        }
        private void HandleDeath()
        {
            Debug.LogWarning("[Unity View] TRIGGER GAME OVER: Marine has been killed");
            if (gameOverScreen != null)
            {
                gameOverScreen.ShowGameOver();
            }

        }
        public void AddExperience(float amount)
        {
            if (IsDead) return;

           
            logicData.AddExperience(amount);

            
            currentMaxExp = logicData.Level * 100f;

            Debug.Log($"[PlayerController] Otrzymano {amount} EXP. Łącznie: {logicData.Experience} / {currentMaxExp}. Poziom: {logicData.Level}");

            
            if (expBar != null)
            {
                expBar.UpdateBar(logicData.Experience, currentMaxExp);
            }

            
            if (logicData.Experience >= currentMaxExp)
            {
                LevelUp();
            }
        }
        private void LevelUp()
        {
            Debug.LogWarning($"[PlayerController] LEVEL UP! Obecny poziom: {logicData.Level}");
            
        }


    }
} 