using System;
using ExtinctionMarine.Gameplay.Systems;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExtinctionMarine.Gameplay.Controllers
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Scene Configuration")]
        [Tooltip("Enter the name of scene (ex. CoreGame)")]
        [SerializeField] private string gameSceneName = "SampleScene";

        [Header("UI Panels")]
        [Tooltip("Connect main menu here (Play, Armory, Settings, Quit, etc...)")]
        [SerializeField] private GameObject mainPanel;

        [Tooltip("Connect Armory panel here")]
        [SerializeField] private GameObject armoryPanel;


        [Tooltip("Connect Setting panel here ")]
        [SerializeField] private GameObject settingsPanel;

        [Header("Version Display")]
        [SerializeField] private TMP_Text versionText;

        private void Start()
        {
            if(versionText != null)
            {
                versionText.text = $"v{Application.version}";
            }
            Time.timeScale = 1f;
            ShowPanel(mainPanel);
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayMenuMusic();
            }

        }

        public void OpenArmory()
        {
            Debug.Log("[MainMenu] Accessing Armory Database...");
            ShowPanel(armoryPanel);
        }

        public void OpenSetting()
        {
            Debug.Log("[MainMenu] Accessing Setting...");
            ShowPanel(settingsPanel);
        }
        public void BackToMainMenu()
        {
            ShowPanel(mainPanel);
        }
        public void StartGame()
        {
            Debug.Log("[MainMenu] Deploying Marine to the hazard zone...");
            SceneManager.LoadScene(gameSceneName);
        }

        public void QuitGame()
        {
            Debug.Log("[MainMenu] Aborting mission. Shutting down.");
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        private void ShowPanel(GameObject panelToShow)
        {
            if (mainPanel != null) mainPanel.SetActive(mainPanel == panelToShow);
            if (armoryPanel != null) armoryPanel.SetActive(armoryPanel == panelToShow);
            if (settingsPanel != null) settingsPanel.SetActive(settingsPanel == panelToShow);

        }
    }
}