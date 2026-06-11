using System.Collections;
using UnityEngine;

namespace ExtinctionMarine.Gameplay
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private EnemyController enemyPrefab;
        [SerializeField] private Transform playerTransform;

        [Header("Spawn Settings")]
        [SerializeField] private float spawnInterval = 1.5f; 
        [SerializeField] private float spawnRadius = 30f;  
        private void Start()
        {
            if (playerTransform == null || enemyPrefab == null)
            {
                Debug.LogError("[EnemySpawner] Missing dependencies! Assign Prefab and Player in Inspector.");
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

          
            EnemyController newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

           
            newEnemy.SetTarget(playerTransform);


        }

        //DEBUG
        // Draws debugging shapes in the Unity Scene view (not visible in the final built game)
        private void OnDrawGizmos()
        {
            if (playerTransform != null)
            {
                Gizmos.color = Color.red;
                // Draws a wireframe sphere showing the exact spawn radius
                Gizmos.DrawWireSphere(playerTransform.position, spawnRadius);
            }
        }
    }
}