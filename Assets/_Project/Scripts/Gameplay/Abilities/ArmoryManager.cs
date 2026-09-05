using System;
using ExtinctionMarine.Gameplay.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace ExtinctionMarine.Gameplay.Abilities
{
    public class ArmoryManager : MonoBehaviour
    {
        [Header("Ability UI Elements")]
        [Tooltip("Icon Button of Ability in menu")]
        [SerializeField] private Button dashButton;

        [Tooltip("Status Text")]
        [SerializeField] private TMP_Text dashStatusText;
        private ArmorySaveData currentData;

        private void OnEnable()
        {
            RefreshArmoryState();
        }

        public void RefreshArmoryState()
        {
            currentData = SaveSystem.Load<ArmorySaveData>("marine_armory.json");

            if (currentData.IsDashUnlocked)
            {
                if (currentData.EquippedAbility == "DASH")
                {
                    dashStatusText.text = "EQUIPPED";
                    dashButton.interactable = false; 
                }
                else
                {
                    dashStatusText.text = "EQUIP";
                    dashButton.interactable = true; 
                }
            }
            else
            {
                dashStatusText.text = $"KILL CARNOTAURUS ({currentData.CarnotaurusKills}/1)";
                dashButton.interactable = false;
            }
        }

        
        public void OnEquipDashClicked()
        {
            currentData.EquippedAbility = "DASH";

            SaveSystem.Save(currentData, "marine_armory.json");

            RefreshArmoryState();
            Debug.Log("[Armory] Dash has been equipped!");
        }
    }
}
    


