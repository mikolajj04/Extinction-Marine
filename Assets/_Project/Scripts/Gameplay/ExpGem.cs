using UnityEngine;
using System;

namespace ExtinctionMarine.Gameplay
{
    public class ExpGem : MonoBehaviour
    {
        [SerializeField] private float expAmount = 10f;
        [SerializeField] private float startMoveSpeed = 4f;
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float collectionDistance = 0.5f;

        private Action<ExpGem> returnToPool;


        private Transform targetPlayer;
        private bool isAttracted;
        private float currentSpeed;

        public void Initialize(Vector3 position, Action<ExpGem> onCollectCallback)
        {
            transform.position = position;
            returnToPool = onCollectCallback;
            isAttracted = false;
            targetPlayer = null;
            currentSpeed = startMoveSpeed;
            gameObject.SetActive(true);
        }

        private void Update()
        {
            
            if (!isAttracted || targetPlayer == null) return;

            
            currentSpeed += acceleration * Time.deltaTime;

            
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, currentSpeed * Time.deltaTime);

            
            if (Vector3.Distance(transform.position, targetPlayer.position) < collectionDistance)
            {
                CollectGem();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
           
            if (isAttracted) return;

            
            PlayerController player = collision.GetComponentInParent<PlayerController>();

            if (player != null)
            {
                
                isAttracted = true;
                targetPlayer = player.transform;
            }
        }

        private void CollectGem()
        {
           
            if (targetPlayer != null && targetPlayer.TryGetComponent<PlayerController>(out var player))
            {
                player.AddExperience(expAmount);
            }

            gameObject.SetActive(false);
            returnToPool?.Invoke(this);
        }
    }
}