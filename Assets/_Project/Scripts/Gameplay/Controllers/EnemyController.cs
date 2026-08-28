using System;
using System.Collections;
using GameLogic.Core.Models;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace ExtinctionMarine.Gameplay.Controllers
{
    public enum DinosaurSpecies
    {
        Raptor,
        TRex,
        Triceratops,
        MicroRaptor,
        Carnotaurus,
        Diplodocus,
        Stegosaurus,
        Dilophosaurus

    }
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))] 
    public class EnemyController : MonoBehaviour
    {
        [Header("")]
        [Header("Visuals")]
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer spriteRenderer;
        private Vector3 baseScale;
        [SerializeField] private SpriteRenderer shadowRenderer;
        private float originalShadowAlpha;
        [SerializeField] private Color hitFlashColor = new Color(1f, 0f, 0f, 0.5f);
        [SerializeField] private float flashDuration = 0.1f;
        private Color originalColor;
        private Coroutine flashCoroutine;
        private int specialAbilityHash;
        private bool hasSpecialAbilityParam;
        private bool hasAttackParam;
        private int attackTriggerHash;

        [Header("Identity")]
        [Tooltip("Choose dinosaur spiecies!")]
        [SerializeField] private DinosaurSpecies species;
        
        [Header("Combat Settings")]
       
        [SerializeField] private float attackCooldown = 1f;

        [Header("Swarm Behavior")]
        [SerializeField] private float separationRadius = 1.0f;
        [SerializeField] private float separationWeight = 1.5f;
        public static event Action<Vector3, float> OnEnemyKilled;
        private Transform playerTransform;
        private PlayerController player;
        private DinosaurEntity logicData;
        private Rigidbody2D rb;
        private Collider2D myCollider;
        private Action<EnemyController> returnToPool;
        private float nextAttackTime = 0f;
        private Collider2D[] separationBuffer = new Collider2D[20];
        private float knockbackTimer = 0f;
        public bool IsImpenetrable => logicData != null && logicData.IsImpenetrable;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            myCollider = GetComponent<Collider2D>();
            baseScale = transform.localScale;

            if(spriteRenderer!= null)
            {
                originalColor = spriteRenderer.color;   
            }
            if (shadowRenderer != null)
            {
                originalShadowAlpha = shadowRenderer.color.a;
            }
            specialAbilityHash = Animator.StringToHash("IsUsingSpecialAbility");
            attackTriggerHash = Animator.StringToHash("Attack");

            if (animator != null)
            {
                foreach (var param in animator.parameters)
                {
                    if (param.nameHash == specialAbilityHash)
                    {
                        hasSpecialAbilityParam = true;           
                    }
                    if (param.nameHash == attackTriggerHash)
                    {
                        hasAttackParam = true; 
                    }
                }
            }
        }

        private DinosaurEntity CreateEntityModel()
        {
            return species switch
            {
                DinosaurSpecies.Raptor => new RaptorEntity(),
                DinosaurSpecies.TRex => new TRexEntity(),
                DinosaurSpecies.Triceratops => new TriceratopsEntity(),
                DinosaurSpecies.MicroRaptor => new MicroRaptorEntity(),
                DinosaurSpecies.Carnotaurus => new CarnotaurusEntity(),
                DinosaurSpecies.Diplodocus => new DiplodocusEntity(),
                DinosaurSpecies.Stegosaurus => new StegosaurusEntity(),
                DinosaurSpecies.Dilophosaurus => new DilophosaurusEntity(),
                _ => new RaptorEntity() 
            };
        }

        public void Initialize(Transform target, Action<EnemyController> onDeathCallback)
        {
            playerTransform = target;
            player = target.GetComponent<PlayerController>();
            returnToPool = onDeathCallback;

            if (logicData == null)
            {
                logicData = CreateEntityModel();
            }
            logicData.ResetEntity();
            rb.linearVelocity = Vector2.zero;
            nextAttackTime = 0f;
            knockbackTimer = 0f;

            flashCoroutine = null;
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }

            gameObject.SetActive(true);
        }

        public void ApplyKnockback(Vector2 knockbackForce)
        {
            if (logicData == null || logicData.IsDead || logicData.IsImmuneToKnockback) return;

            
            rb.linearVelocity = knockbackForce;

            
            knockbackTimer = 0.2f;
        }
        private void FixedUpdate()
        {        
            if (logicData == null || logicData.IsDead || playerTransform == null)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            logicData.Tick(Time.fixedDeltaTime);
            if (knockbackTimer > 0f)
            {
                knockbackTimer -= Time.fixedDeltaTime;       
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.fixedDeltaTime * 5f);

                return; 
            }

            Vector2 targetPosition = playerTransform.position;
            if (logicData.IsSneaky)
            {
                Vector2 toEnemy = (Vector2)transform.position - (Vector2)playerTransform.position;
                float distanceToPlayer = toEnemy.magnitude;
                float dotProduct = Vector2.Dot(player.AimDirection, toEnemy.normalized);

                if (dotProduct <= logicData.AttackConeThreshold || distanceToPlayer <= 4f)
                {
                    targetPosition = playerTransform.position; 
                    Debug.DrawLine(transform.position, targetPosition, Color.red);
                }
                else
                {
                    float side = (player.AimDirection.x * toEnemy.y - player.AimDirection.y * toEnemy.x) > 0 ? 1f : -1f;

                    Vector2 perpendicular = new Vector2(-player.AimDirection.y, player.AimDirection.x) * side;
                    Vector2 bypassPoint = (Vector2)playerTransform.position - (player.AimDirection * 1f) + (perpendicular * 10f);

                    targetPosition = bypassPoint;
                    Debug.DrawLine(transform.position, targetPosition, Color.blue);
                }
            }

            

            Vector2 directionToTarget = (targetPosition - (Vector2)transform.position).normalized;
            Vector2 separationForce = Vector2.zero;
               
            float scanRadius = separationRadius + 5f;
            int hitCount = Physics2D.OverlapCircle(transform.position, scanRadius,ContactFilter2D.noFilter, separationBuffer);

            for (int i = 0; i < hitCount; i++)
            {
                Collider2D neighbor = separationBuffer[i];

                if (neighbor.gameObject != gameObject && (neighbor.TryGetComponent<EnemyController>(out _) || neighbor.CompareTag("Obstacle")))
                {
                    ColliderDistance2D colDist = Physics2D.Distance(myCollider, neighbor);
                    float trueDistance = colDist.isOverlapped ? 0f : colDist.distance;

                    if (trueDistance < separationRadius)
                    {
                        
                        Vector2 pushDir = (myCollider.bounds.center - neighbor.bounds.center).normalized;

                       
                        if (pushDir == Vector2.zero)
                        {
                            pushDir = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
                        }

                       
                        float mySize = myCollider.bounds.extents.sqrMagnitude;
                        float neighborSize = neighbor.bounds.extents.sqrMagnitude;

                        if (mySize < 0.1f) mySize = 0.1f;


                        float sizeRatio = neighbor.CompareTag("Obstacle") ? 10f : Mathf.Clamp(neighborSize / mySize, 0.1f, 5f);


                        float pushStrength = 1f - (trueDistance / separationRadius);

                     
                        if (colDist.isOverlapped)
                        {
                            pushStrength *= 2f;
                        }

                      
                        separationForce += pushDir * (pushStrength * sizeRatio);
                    }
                }
            }
            
            Vector2 finalDirection = (directionToTarget + (separationForce * separationWeight)).normalized;
            Vector2 targetVelocity = finalDirection * logicData.Speed;

            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * logicData.Agility);
        }

         

        public void TakeDamage(float amount)
        {
            if (logicData == null || logicData.IsDead) return;

            logicData.TakeDamage(amount);
            Debug.Log($"[EnemyController] {species} took {amount} damage. HP: {logicData.CurrentHealth}");
            if (spriteRenderer != null)
            {
                if (flashCoroutine != null)
                {
                    StopCoroutine(flashCoroutine);
                    spriteRenderer.color = originalColor;
                }
                flashCoroutine = StartCoroutine(FlashRoutine());
            }
            if (logicData.IsDead)
            {
                Die();
            }
        }
        
        private void Die()
        {
            Debug.Log($"[EnemyController] {species} eliminated, recycling into pool.");
            OnEnemyKilled?.Invoke(transform.position, logicData.XpReward);
            returnToPool?.Invoke(this);

        }

        private void OnCollisionStay2D(Collision2D collision)
        {

            if (logicData == null || logicData.IsDead) return;


            if (Time.time >= nextAttackTime)
            {
                if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
                {
                    player.ApplyDamage(logicData.Damage);

                    if (logicData.MeleeKnockbackForce > 0f)
                    {
                        
                        Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
                        Vector2 knockbackVector = knockbackDirection * logicData.MeleeKnockbackForce;

                        player.ApplyKnockback(knockbackVector);
                    }

                    nextAttackTime = Time.time + attackCooldown;

                    if(animator != null && hasAttackParam)
                    {
                        animator.SetTrigger(attackTriggerHash);
                    }

                    Debug.Log($"[EnemyController] {species} bites the player for {logicData.Damage} damage!");
                }
            }
        }
        private void Update()
        {
            UpdateVisuals();
            if (animator != null && logicData != null && hasSpecialAbilityParam)
            {
                animator.SetBool(specialAbilityHash, logicData.IsUsingSpecialAbility);
            }

        }

        private void UpdateVisuals()
        {
            if (playerTransform == null) return;


            if (playerTransform.position.x > transform.position.x)
            {
          
                transform.localScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
            }
            else if (playerTransform.position.x < transform.position.x)
            {
  
                transform.localScale = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
            }
            if (spriteRenderer != null && flashCoroutine == null)
            {
                Color currentColor = spriteRenderer.color;

                
                currentColor.a = Mathf.Lerp(currentColor.a, logicData.TargetAlpha, Time.deltaTime * 3f);
                spriteRenderer.color = currentColor;

                originalColor = new Color(originalColor.r, originalColor.g, originalColor.b, currentColor.a);
            }
            if (shadowRenderer != null)
            {
                Color shadowColor = shadowRenderer.color;
                shadowColor.a = Mathf.Lerp(shadowColor.a, originalShadowAlpha * logicData.TargetAlpha, Time.deltaTime * 3f);
                shadowRenderer.color = shadowColor;
            }
        }

        private IEnumerator FlashRoutine()
        {

            spriteRenderer.color = hitFlashColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = originalColor;

            flashCoroutine = null; 
        }

    }
}