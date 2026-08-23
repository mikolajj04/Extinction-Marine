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
        Stegosaurus
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
        [SerializeField] private Color hitFlashColor = new Color(1f, 0f, 0f, 0.5f);
        [SerializeField] private float flashDuration = 0.1f;
        private Color originalColor;
        private Coroutine flashCoroutine;

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
                _ => new RaptorEntity() 
            };
        }

        public void Initialize(Transform target, Action<EnemyController> onDeathCallback)
        {
            playerTransform = target;
            returnToPool = onDeathCallback;

            logicData = CreateEntityModel();

            rb.linearVelocity = Vector2.zero;
            nextAttackTime = 0f;

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

            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
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

            Vector2 targetDirection = (directionToPlayer + (separationForce * separationWeight)).normalized;
            Vector2 targetVelocity = targetDirection * logicData.Speed;

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

                    if(animator != null)
                    {
                        animator.SetTrigger("Attack");
                    }

                    Debug.Log($"[EnemyController] {species} bites the player for {logicData.Damage} damage!");
                }
            }
        }
        private void Update()
        {
            UpdateVisuals();
            if (animator != null && logicData != null)
            {
                animator.SetBool("IsUsingSpecialAbility", logicData.IsUsingSpecialAbility);
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