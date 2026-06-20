using GameLogic.Core.Models;
using UnityEngine;
using UnityEngine.InputSystem;
using ExtinctionMarine.Gameplay.UI;
using System;

namespace ExtinctionMarine.Gameplay
{
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private HealthBar expBar; 
        [Header("UI Dependencies")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private GameOverScreen gameOverScreen;
        

        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 5f;

        public float MoveSpeed { get; private set; }

        [Header("Combat Dependencies")]
        [SerializeField] private ProjectilePool projectilePool;
        [SerializeField] private float fireRate = 0.2f;
        public float FireRate { get; private set; }

        private PlayerEntity logicData;

        public PlayerEntity LogicData => logicData;
        private Rigidbody2D rb;
        private Vector2 moveInput;
        private Camera mainCamera;
        public static event Action OnPlayerLevelUp;

        private bool isFiring;
        private float fireCooldownTimer;

        public float CurrentHp => logicData.CurrentHealth;
        public float MaxHp => logicData.MaxHealth;
        public bool IsDead => logicData.IsDead;


       //Upgrade Gates:

        public void ApplyFireRateUpgrade(float amount)
        {
            FireRate -= amount;
            Debug.LogWarning($"[PlayerController] Upgrade has been choosen!: Fire rate increased to {FireRate}!");

        }
        public void ApplyHeal(float amount)
        {
            if (logicData == null || IsDead) return;

            
            logicData.Heal(amount);

            
            if (healthBar != null)
            {
                healthBar.UpdateBar(logicData.CurrentHealth, logicData.MaxHealth);
            }

            Debug.Log($"[PlayerController]pgrade has been choosen!: Healed by {amount}, current HP: {logicData.CurrentHealth}");
        }

        public void ApplyMaxHealthIncrease(float amount)
        {
            if (logicData == null || IsDead) return;
            logicData.IncreaseMaxHealth(amount);
            if (healthBar != null)
            {
                healthBar.UpdateBar(logicData.CurrentHealth, logicData.MaxHealth);
                Debug.LogWarning($"[PlayerController] Upgrade has been choosen!: Marine max-health increased! current HP: {logicData.MaxHealth}");
            }
        }

        public void ApplySpeedUpgrade(float amount)
        {
            MoveSpeed += amount;
            Debug.LogWarning($"[PlayerController] Upgrade has been choosen!: Marine speed increased to {MoveSpeed}!");
        }

        
        
      
      
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

            UpdateExpUI();
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
                return;
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

        private void UpdateExpUI()
        {
            if (expBar == null) return;

            float expNeededForCurrentLevel = GetExpRequiredForLevel(logicData.Level);
            float previousLevelMaxExp = GetExpRequiredForLevel(logicData.Level-1);

            float expProgressInThisLevel = logicData.Experience - previousLevelMaxExp;
            float totalExpRequiredForThisLevel = expNeededForCurrentLevel - previousLevelMaxExp;

            expBar.UpdateBar(expProgressInThisLevel, totalExpRequiredForThisLevel);
        }
        public void AddExperience(float amount)
        {
            if (IsDead) return;

            logicData.AddExperience(amount);


            float expNeededForCurrentLevel = GetExpRequiredForLevel(logicData.Level);

           
            if (logicData.Experience >= expNeededForCurrentLevel)
            {
                logicData.LevelUp();
                OnPlayerLevelUp?.Invoke();
            }

           
            UpdateExpUI();
        }



        private float GetExpRequiredForLevel(int level)
        {
            if (level <= 0) return 0f;
            return level * 50f * (level + 0.5f);
        }

    }


} 
