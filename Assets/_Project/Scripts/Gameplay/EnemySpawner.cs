using System.Collections;
using UnityEngine;

namespace ExtinctionMarine.Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private EnemyPool enemyPool; 
        [SerializeField] private Transform playerTransform;

        [Header("Spawn Settings")]
        [SerializeField] private float spawnInterval = 1f;
        [SerializeField] private float spawnRadius = 30f;

        private void Start()
        {
            if (playerTransform == null || enemyPool == null)
            {
                Debug.LogError("[EnemySpawner] Missing dependencies! Assign Pool and Player.");
                return;
            }
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            Vector3 spawnPosition = playerTransform.position + (Vector3)(randomDirection * spawnRadius);

           
            EnemyController newEnemy = enemyPool.GetEnemy(spawnPosition);

         
            newEnemy.Initialize(playerTransform, enemyPool.ReturnEnemy);
        }

        private void OnDrawGizmos()
        {
            if (playerTransform != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(playerTransform.position, spawnRadius);
            }
        }
    }
}