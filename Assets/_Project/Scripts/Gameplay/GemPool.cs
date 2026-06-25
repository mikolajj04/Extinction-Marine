using System.Collections.Generic;
using UnityEngine;

namespace ExtinctionMarine.Gameplay
{
    public class GemPool : MonoBehaviour
    {
        [SerializeField] private ExpGem gemPrefab;
        [SerializeField] private int initialSize = 50;

        private Queue<ExpGem> pool = new Queue<ExpGem>();

      
        private void OnEnable()
        {
            EnemyController.OnEnemyKilled += SpawnGemAtPosition;
        }

        private void OnDisable()
        {
            EnemyController.OnEnemyKilled -= SpawnGemAtPosition;
        }

        private void Start()
        {
            for (int i = 0; i < initialSize; i++)
            {
                CreateNewGem();
            }
        }

        private ExpGem CreateNewGem()
        {
            ExpGem gem = Instantiate(gemPrefab, transform);
            gem.gameObject.SetActive(false);
            pool.Enqueue(gem);
            return gem;
        }

        private void SpawnGemAtPosition(Vector3 deathPosition, float xpAmount)
        {
            ExpGem gem = pool.Count > 0 ? pool.Dequeue() : CreateNewGem();
            gem.Initialize(deathPosition, ReturnGem, xpAmount);
        }

        private void ReturnGem(ExpGem gem)
        {
            if (!pool.Contains(gem))
            {
                pool.Enqueue(gem);
            }
        }
    }
}