using System.Collections.Generic;
using UnityEngine;

namespace ExtinctionMarine.Gameplay
{
    public class ProjectilePool : MonoBehaviour
    {
        [Header("Pool Configuration")]
        [SerializeField] private ProjectileController projectilePrefab;
        [SerializeField] private int initialPoolSize = 50;

        private Queue<ProjectileController> poolQueue = new Queue<ProjectileController>();

        private void Start()
        {
            PreWarmPool();
        }

        
        private void PreWarmPool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                ProjectileController bullet = Instantiate(projectilePrefab, transform);
                bullet.gameObject.SetActive(false);
                poolQueue.Enqueue(bullet);
            }
        }

     
        public void FireProjectile(Vector2 position, Vector2 direction, float damage, int pierceCount)
        {
            ProjectileController bullet;

         
            if (poolQueue.Count > 0)
            {
                bullet = poolQueue.Dequeue();
            }
            else
            {
                Debug.LogWarning("[ProjectilePool] Pool is out! Dynamic allocation of a new projectile.");
                bullet = Instantiate(projectilePrefab, transform);
            }

          
            bullet.Initialize(position, direction, damage,pierceCount, ReturnToPool);
        }

      
        private void ReturnToPool(ProjectileController bullet)
        {
         
            if (!poolQueue.Contains(bullet))
            {
                poolQueue.Enqueue(bullet);
            }
        }
    }
}