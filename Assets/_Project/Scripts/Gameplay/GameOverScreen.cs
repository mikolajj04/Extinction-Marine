using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

namespace ExtinctionMarine.Gameplay.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Text killCountText;
        [SerializeField] private Text survivalTimeText;
        private int currentKills = 0;
        private void OnEnable()
        {
            EnemyController.OnEnemyKilled += RegisterKill;
        }
        private void OnDisable()
        {
            
            EnemyController.OnEnemyKilled -= RegisterKill;
        }
        private void RegisterKill(Vector3 deathPosition)
        {
            currentKills++;
        }

        private void Start()
        {
            
            gameOverPanel.SetActive(false);
        }

        public void ShowGameOver()
        {
            gameOverPanel.SetActive(true);

            TimeSpan time = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
            survivalTimeText.text = $"YOU SURVIVED: {time.Minutes:D2}:{time.Seconds:D2}";
            killCountText.text = $"RAPTORS KILLED: {currentKills}";

            Time.timeScale = 0f;

            Debug.Log("[GameOverScreen] Game Over triggered. Time frozen.");
        }

        public void RestartGame()
        {
           
            Time.timeScale = 1f;

            Debug.Log("[GameOverScreen] Restarting current scene...");

          
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}