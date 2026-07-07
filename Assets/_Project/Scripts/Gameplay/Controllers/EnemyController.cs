using System;
using UnityEngine;
using GameLogic.Core.Models;


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
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            myCollider = GetComponent<Collider2D>();
            
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
            gameObject.SetActive(true);
        }

        private void FixedUpdate()
        {
            if (logicData == null || logicData.IsDead || playerTransform == null)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Vector2 separationForce = Vector2.zero;

            float scanRadius = separationRadius + 5f;
            int hitCount = Physics2D.OverlapCircle(transform.position, scanRadius,ContactFilter2D.noFilter, separationBuffer);

            for (int i = 0; i < hitCount; i++)
            {
                Collider2D neighbor = separationBuffer[i];

                if (neighbor.gameObject != gameObject && neighbor.TryGetComponent<EnemyController>(out _))
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

                      
                        float sizeRatio = Mathf.Clamp(neighborSize / mySize, 0.1f, 5f);

                      
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

            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * 12f);
        }




        public void TakeDamage(float amount)
        {
            if (logicData == null || logicData.IsDead) return;

            logicData.TakeDamage(amount);
            Debug.Log($"[EnemyController] {species} took {amount} damage. HP: {logicData.CurrentHealth}");

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


                    nextAttackTime = Time.time + attackCooldown;

                    Debug.Log($"[EnemyController] Raptor bites the player for {logicData.Damage} damage!");
                }
            }
        }
    }
}