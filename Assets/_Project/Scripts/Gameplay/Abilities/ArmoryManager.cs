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
        [Tooltip("Icon/Button of Ability in menu")]
        [SerializeField] private Button dashButton;
        [SerializeField] private Button noneButton;

        [Tooltip("Status Text")]
        [SerializeField] private TMP_Text dashStatusText;
        [SerializeField] private TMP_Text noneStatusText;
        private ArmorySaveData currentData;

        private void OnEnable()
        {
            RefreshArmoryState();
        }

        private void RefreshArmoryState()
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
            if(currentData.EquippedAbility == "NONE")
            {
                noneStatusText.text = "EQUIPPED";
                noneButton.interactable = false;
            }else if (currentData.EquippedAbility == "")
            {
                noneStatusText.text = "EQUIPPED";
                noneButton.interactable = false;
            }
            else 
            {
                noneStatusText.text = "EQUIP";
                noneButton.interactable = true;
            }
            
        }

        
        public void OnEquipDashClicked()
        {
            currentData.EquippedAbility = "DASH";

            SaveSystem.Save(currentData, "marine_armory.json");

            RefreshArmoryState();
            Debug.Log("[Armory] Dash has been equipped!");
        }

        public void OnEquipNoneClicked()
        {
            currentData.EquippedAbility = "NONE";

            SaveSystem.Save(currentData, "marine_armory.json");

            RefreshArmoryState();
            Debug.Log("[Armory] NONE has been equipped!");
        }
    }
}
    


