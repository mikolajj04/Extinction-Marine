using System;
using System.IO;
using ExtinctionMarine.Gameplay.Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ExtinctionMarine.Gameplay.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Text killCountText;
        [SerializeField] private Text survivalTimeText;
        private int currentKills = 0;
        private string saveFilePath;

        private void Awake()
        {
            saveFilePath = Path.Combine(Application.persistentDataPath, "highscore.txt");
        }
        private void OnEnable()
        {
            EnemyController.OnEnemyKilled += RegisterKill;
        }
        private void OnDisable()
        {
            
            EnemyController.OnEnemyKilled -= RegisterKill;
        }
        private void RegisterKill(Vector3 deathPosition, float xpAmount)
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
            killCountText.text = $"ENEMIES KILLED: {currentKills}";

            Time.timeScale = 0f;
            SaveScoreToFilesystem();

            Debug.Log("[GameOverScreen] Game Over triggered. Time frozen.");
        }

        public void RestartGame()
        {
           
            Time.timeScale = 1f;

            Debug.Log("[GameOverScreen] Restarting current scene...");

          
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        private void SaveScoreToFilesystem()
        {
            try
            {
                string timeOfDeath = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logEntry = $"MARINE DIED AT: {timeOfDeath}.\nKILLED ENEMIES: {currentKills}.\n{survivalTimeText.text}. \n--------------------";

             
                File.AppendAllText(saveFilePath, logEntry);
                Debug.Log($"[File System] Score saved in: {saveFilePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Saving file error: {e.Message}");
            }
        }
    }
}
