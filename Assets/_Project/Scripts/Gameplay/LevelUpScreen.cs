using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;
using ExtinctionMarine.Gameplay.Upgrades;

namespace ExtinctionMarine.Gameplay.UI
{
    public class LevelUpScreen : MonoBehaviour
    {
        [Header("Systems")]
        [SerializeField] private GameObject levelUpPanel;
        [SerializeField] private UpgradeManager upgradeManager; 

        [Header("UI elements [Cards]")]
     
        [SerializeField] private Button[] upgradeButtons;
        [SerializeField] private TextMeshProUGUI[] titleTexts;
        [SerializeField] private TextMeshProUGUI[] descriptionTexts;

       
        private List<IUpgrade> currentChoices;

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

        private void ShowLevelUpScreen()
        {
            Time.timeScale = 0f; 

            
            currentChoices = upgradeManager.GetRandomUpgrades(upgradeButtons.Length);

          
            for (int i = 0; i < upgradeButtons.Length; i++)
            {
                if (i < currentChoices.Count)
                {
                    
                    upgradeButtons[i].gameObject.SetActive(true);
                    titleTexts[i].text = currentChoices[i].Title;
                    descriptionTexts[i].text = currentChoices[i].Description;
                }
                else
                {
                    
                    upgradeButtons[i].gameObject.SetActive(false);
                }
            }

            levelUpPanel.SetActive(true);
        }

       
        public void OnUpgradeButtonClicked(int buttonIndex)
        {
            if (buttonIndex >= currentChoices.Count) return;

            
            IUpgrade chosenUpgrade = currentChoices[buttonIndex];
            upgradeManager.SelectUpgrade(chosenUpgrade);

            
            levelUpPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}