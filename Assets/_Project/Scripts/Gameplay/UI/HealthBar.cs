using UnityEngine;
using UnityEngine.UI;

namespace ExtinctionMarine.Gameplay.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;

        
        public void UpdateBar(float currentHealth, float maxHealth)
        {
            
            fillImage.fillAmount = currentHealth / maxHealth;

            Debug.Log($"[HealthBar] UI updated. Current fill: {fillImage.fillAmount}");
        }
    }
}