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

        [Tooltip("Status Text")]
        [SerializeField] private TMP_Text dashStatusText;
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
                dashButton.interactable = true;
                if (dashStatusText != null) dashStatusText.text = "UNLOCKED";
            }
            else
            {
                dashButton.interactable = false;
                if (dashStatusText != null) dashStatusText.text = $"KILL CARNOTAURUS ({currentData.CarnotaurusKills}/1)";
            }
        }
    }
}

