using UnityEngine;
using System;

namespace ExtinctionMarine.Gameplay.UI
{
    public class LevelUpScreen : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject levelUpPanel;

        public static event Action OnFireRateUpgrade;
        public static event Action OnSpeedUpgrade;


        private void OnEnable()
        {
            
            PlayerController.OnPlayerLevelUp += ShowLevelUpScreen;
        }
        private void OnDisable()
        {
            PlayerController.OnPlayerLevelUp -= ShowLevelUpScreen;
        }
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
            OnFireRateUpgrade?.Invoke();
            ResumeGame();
        }

        public void OnSpeedUpgradeChosen()
        {
            Debug.Log("[LevelUpScreen] Speed upgrade selected.");
            OnSpeedUpgrade?.Invoke();
            ResumeGame();
        }

        private void ResumeGame()
        {
            levelUpPanel.SetActive(false);

           
            Time.timeScale = 1f;
        }
    }
}