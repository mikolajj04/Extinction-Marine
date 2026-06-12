using System.Collections.Generic;
using UnityEngine;

namespace ExtinctionMarine.Gameplay
{
    public class EnemyPool : MonoBehaviour
    {
        [Header("Pool Config")]
        [SerializeField] private EnemyController enemyPrefab;
        [SerializeField] private int initialSize = 30;

        private Queue<EnemyController> pool = new Queue<EnemyController>();

        private void Start()
        {
         
            for (int i = 0; i < initialSize; i++)
            {
                CreateNewEnemy();
            }
        }

        private EnemyController CreateNewEnemy()
        {
            EnemyController enemy = Instantiate(enemyPrefab, transform);
            enemy.gameObject.SetActive(false);
            pool.Enqueue(enemy);
            return enemy;
        }

        public EnemyController GetEnemy(Vector3 position)
        {
          
            EnemyController enemy = pool.Count > 0 ? pool.Dequeue() : CreateNewEnemy();

            enemy.transform.position = position;
            return enemy;
        }

        public void ReturnEnemy(EnemyController enemy)
        {
            enemy.gameObject.SetActive(false);
            pool.Enqueue(enemy);
        }
    }
}