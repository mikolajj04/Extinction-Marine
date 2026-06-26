using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ExtinctionMarine.Gameplay.Spawning
{
  
    [Serializable]
    public class WaveConfig
    {
        public string WaveName = "Phase 1";
        [Tooltip("At what second of the game (starting from zero) does this wave begin?")]
        public float TimeToStart;
        [Tooltip("How often do we spawn a new enemy?")]
        public float SpawnRate;
        [Tooltip("List of pools (EnemyPool) from which the manager can draw in this wave")]
        public EnemyPool[] AllowedPools;

        [Header("Boss Event (Optional)")]
        [Tooltip("Is the Boss supposed spawn in the moment this wave starts?")]
        public bool SpawnBossOnStart;
        [Tooltip("The Boss prefab")]
        public EnemyController BossPrefab;
    }

    public class WaveManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerController player;

        [Header("Wave Schedule")]
        [SerializeField] private List<WaveConfig> waves;
        [SerializeField] private float spawnRadius = 15f; 

        private float gameTimer = 0f;
        private float spawnTimer = 0f;
        private int currentWaveIndex = 0;
        private WaveConfig currentWave;

        private void Start()
        {
            if (waves != null && waves.Count > 0)
            {
                currentWave = waves[0];
                Debug.Log($"[WaveManager] System armed. Let's begin: {currentWave.WaveName}");
            }
            else
            {
                Debug.LogError("[WaveManager] No waves have been configured in the schedule!");
            }
        }

        private void Update()
        {
            if (player == null || player.IsDead || currentWave == null) return;

            gameTimer += Time.deltaTime;
            spawnTimer -= Time.deltaTime;

            CheckForNextWave();

           
            if (spawnTimer <= 0f)
            {
                SpawnRegularEnemy();
                spawnTimer = currentWave.SpawnRate;
            }
        }

        private void CheckForNextWave()
        {
         
            if (currentWaveIndex + 1 < waves.Count)
            {
                WaveConfig nextWave = waves[currentWaveIndex + 1];

            
                if (gameTimer >= nextWave.TimeToStart)
                {
                    currentWaveIndex++;
                    currentWave = nextWave;
                    Debug.LogWarning($"[WaveManager] Time to go on! We're entering: {currentWave.WaveName}");

                  
                    if (currentWave.SpawnBossOnStart && currentWave.BossPrefab != null)
                    {
                        SpawnBoss();
                    }
                }
            }
        }

        private void SpawnRegularEnemy()
        {
            if (currentWave.AllowedPools == null || currentWave.AllowedPools.Length == 0) return;

            
            int randomIndex = Random.Range(0, currentWave.AllowedPools.Length);
            EnemyPool selectedPool = currentWave.AllowedPools[randomIndex];

            Vector2 spawnPos = GetSpawnPositionOffScreen();

           
            EnemyController enemy = selectedPool.GetEnemy(spawnPos);

          
            enemy.Initialize(player.transform, (e) => selectedPool.ReturnEnemy(e));
        }

        private void SpawnBoss()
        {
            Vector2 spawnPos = GetSpawnPositionOffScreen();

        
            EnemyController boss = Instantiate(currentWave.BossPrefab, spawnPos, Quaternion.identity);

            boss.Initialize(player.transform, (e) => Destroy(e.gameObject));

            Debug.LogWarning($"[WaveManager]  ATTENTION! MASSIVE BIOLOGICAL ANOMALY:  {currentWave.BossPrefab.name} ON RADAR!");
        }

     
        private Vector2 GetSpawnPositionOffScreen()
        {
            float randomAngle = Random.Range(0f, 360f);
            float radians = randomAngle * Mathf.Deg2Rad;

            float x = Mathf.Cos(radians) * spawnRadius;
            float y = Mathf.Sin(radians) * spawnRadius;

            return new Vector2(player.transform.position.x + x, player.transform.position.y + y);
        }
    }
}