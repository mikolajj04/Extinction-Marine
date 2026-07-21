using System;
using System.Runtime.InteropServices.WindowsRuntime;
using ExtinctionMarine.Gameplay.Pools;
using ExtinctionMarine.Gameplay.UI;
using GameLogic.Core.Models;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExtinctionMarine.Gameplay.Controllers
{
    
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private Transform firePoint;
        [Header("VFX")]
        [SerializeField] Animator animator;
        [Tooltip("Drag the MuzzleFlash (Particle System) object from the player's hierarchy here")]
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private HealthBar expBar; 
        [Header("UI Dependencies")]
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private GameOverScreen gameOverScreen;
        [SerializeField] private TMP_Text levelText;

        [Header("Collection Settings")]
        [SerializeField] private CircleCollider2D magnetCollider;
       
        [Header("Combat Dependencies")]
        [SerializeField] private ProjectilePool projectilePool;
        [SerializeField] private float fireRate = 0.35f;
        public float FireRate { get; private set; } // Fire Cooldown

        private PlayerEntity logicData;
        private Rigidbody2D rb;
        private Vector2 moveInput;
        private Camera mainCamera;
        public static event Action OnPlayerLevelUp;

        private bool isFiring;
        private float fireCooldownTimer;


        public bool IsDead => logicData.IsDead;

       //Upgrade Gates:
        public void ApplyFireRateUpgrade(float percentageAmount)
        {

            FireRate *= (1f - percentageAmount); //FireRate as Fire cooldown
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
            logicData.IncreaseSpeed(amount);
            Debug.LogWarning($"[PlayerController] Upgrade has been choosen!: Marine speed increased to {logicData.MoveSpeed}!");
        }

        public void ApplyMagnetUpgrade(float radiusIncrease)
        {
            if (magnetCollider != null)
            {
                
                magnetCollider.radius += radiusIncrease;
                Debug.LogWarning($"[Upgrade] Nano-Magnet upgraded! Nowy radius: {magnetCollider.radius}");
            }
            else
            {
                Debug.LogError("No Magnet integrated!");
            }
        }

        public void ApplySplitShotUpgrade()
        {
            logicData.AddProjectile();
            Debug.LogWarning($"[PlayerController] Upgrade has been chosen!: Brand new gun installed! Number of guns: {logicData.ProjectileCount}");
        }
        public void ApplyRearGunUpgrade()
        {
            logicData.AddRearProjectile();
            Debug.LogWarning($"[PlayerController] Upgrade has been chosen!: Brand new rear-gun installed! Number of rear-guns: {logicData.RearProjectileCount}");
        }

        public void ApplyDamageUpgrade()
        {
            logicData.IncreaseDamage(2f);
            Debug.LogWarning($"[PlayerController] Upgrade has been chosen!: Bullets has been upgraded. Damage of your projectiles {logicData.Damage}");
        }

        public void ApplyBulletSpeedUpgrade()
        {
            logicData.IncreaseProjectileSpeed(5f);
            Debug.LogWarning($"[PlayerController] Upgrade has been chosen!: Bullets has been upgraded. Current speed of your projectiles {logicData.ProjectileSpeed}");
        }

        public void ApplyPenetrationUpgrade()
        {
            logicData.IncreasePenetration();
            Debug.LogWarning($"[PlayerController] Upgrade has been chosen!: Bullets has been upgraded. Current penetration level of bullets: {logicData.PenetrationCount}");
        
        }

        public void ApplyConcussiveShells()
        {
            logicData.IncreaseKnockback(2.5f);
            Debug.LogWarning($"[PlayerController] Upgrade has been chosen!: Bullets has been upgraded. Current knockback force: {logicData.KnockbackForce}");

        }

        private void UpdateLevelUI()
        {
            if (levelText != null && logicData != null)
            {
                levelText.text = $"LVL {logicData.Level}";
            }
        }

        private void Awake()
        {
            
            rb = GetComponent<Rigidbody2D>();
            mainCamera = Camera.main;
            logicData = new PlayerEntity();
            FireRate = fireRate;
        }

        private void Start()
        {
            if (healthBar != null)
            {
                healthBar.UpdateBar(logicData.CurrentHealth, logicData.MaxHealth);
            }

            UpdateExpUI();
            UpdateLevelUI();
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
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            bool isMoving = moveInput.magnitude > 0.1f;
            if (animator != null)
            {
                animator.SetBool("IsRunning", isMoving);
            }

            RotateTowardsMouse();

            fireCooldownTimer -= Time.deltaTime;

       
            if (isFiring && fireCooldownTimer <= 0f)
            {
                Shoot();
                fireCooldownTimer = FireRate;
            }
            
        }
        private void Shoot()
        {
           
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);

           
            Vector2 baseDirection = (mouseWorldPos - transform.position).normalized;
            FireVolley(baseDirection, logicData.ProjectileCount);
            
            if(logicData.RearProjectileCount > 0)
            {
                FireVolley(-baseDirection, logicData.RearProjectileCount);
            }
            if (muzzleFlash != null)
            {
                muzzleFlash.Play();
            }

            CameraController.Instance.TriggerShake(0.1f, 0.15f);


        }

        private void FireVolley(Vector2 direction, int count)
        {
            if (count <= 1)
            {
                projectilePool.FireProjectile(firePoint.position, direction, logicData.Damage, logicData.ProjectileSpeed, logicData.PenetrationCount, logicData.KnockbackForce);
                return;
            }

            float angleStep = 15f;
            float totalSpread = angleStep * (count - 1);
            float startAngle = -totalSpread / 2f;

            for (int i = 0; i < count; i++)
            {
                float currentAngle = startAngle + (i * angleStep);
                Vector2 rotatedDirection = RotateVector(direction, currentAngle);
                projectilePool.FireProjectile(firePoint.position, rotatedDirection, logicData.Damage, logicData.ProjectileSpeed, logicData.PenetrationCount, logicData.KnockbackForce);
            }
        }

        private Vector2 RotateVector(Vector2 vector, float degrees)
        {
            float radians = degrees * Mathf.Deg2Rad;
            float sin = Mathf.Sin(radians);
            float cos = Mathf.Cos(radians);

            return new Vector2(
                vector.x * cos - vector.y * sin,
                vector.x * sin + vector.y * cos
            );
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
           
            Vector2 movement = moveInput.normalized * logicData.MoveSpeed;

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
                UpdateLevelUI();
                OnPlayerLevelUp?.Invoke();
            }

           
            UpdateExpUI();
        }

        private float GetExpRequiredForLevel(int level)
        {
            if (level <= 0) return 0f;
            return level * 50f * (level + 0.5f);
        }
        private void RotateTowardsMouse()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

         
            Vector2 direction = mousePos - transform.position;

        
            Vector3 characterScale = transform.localScale;

            if (direction.x > 0)
            {
          
                characterScale.x = Mathf.Abs(characterScale.x);
            }
            else if (direction.x < 0)
            {

                characterScale.x = -Mathf.Abs(characterScale.x);
            }

            transform.localScale = characterScale;


            float tiltAngle = Mathf.Atan2(direction.y, Mathf.Abs(direction.x)) * Mathf.Rad2Deg;

            tiltAngle = Mathf.Clamp(tiltAngle, -45f, 45f);

           
            if (direction.x > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, tiltAngle);
            }
            else if (direction.x < 0)
            {
                
                transform.rotation = Quaternion.Euler(0f, 0f, -tiltAngle);
            }
        }
    }

}


