using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExtinctionMarine.Gameplay.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [Header("Scene Configuration")]
        [Tooltip("Enter the name of scene (ex. CoreGame)")]
        [SerializeField] private string gameSceneName = "SampleScene";

        private void Start()
        {
            Time.timeScale = 1f;
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
    }
}