using UnityEngine;

namespace ExtinctionMarine.Gameplay.UI
{
    public class LevelUpScreen : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject levelUpPanel;

        private void Start()
        {
            
            levelUpPanel.SetActive(false);
        }

        public void ShowLevelUpScreen()
        {
            levelUpPanel.SetActive(true);

            
            Time.timeScale = 0f;
            Debug.Log("[LevelUpScreen] System upgrade initialized. Time frozen.");
        }

      
        public void OnFireRateUpgradeChosen()
        {
            Debug.Log("[LevelUpScreen] Fire Rate upgrade selected.");
            
            ResumeGame();
        }

        public void OnSpeedUpgradeChosen()
        {
            Debug.Log("[LevelUpScreen] Speed upgrade selected.");
            
            ResumeGame();
        }

        private void ResumeGame()
        {
            levelUpPanel.SetActive(false);

           
            Time.timeScale = 1f;
        }
    }
}