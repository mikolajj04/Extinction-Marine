using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExtinctionMarine.Gameplay.UI
{
    public class GameOverScreen : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject gameOverPanel;

        private void Start()
        {
            
            gameOverPanel.SetActive(false);
        }

        public void ShowGameOver()
        {
            gameOverPanel.SetActive(true);

            
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