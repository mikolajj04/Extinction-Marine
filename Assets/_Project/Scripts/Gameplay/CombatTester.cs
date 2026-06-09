using UnityEngine;
using UnityEngine.InputSystem;
using GameLogic.Core.Systems;
using GameLogic.Core.Models;

namespace ExtinctionMarine.Gameplay
{
    public class CombatTester : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        private RaptorEntity temporaryRaptor;

        private void Start()
        {
            temporaryRaptor = new RaptorEntity();
            Debug.Log("[CombatTester] Test system started. Press space to inflict damage");
        }

        private void Update()
        {
           
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                ExecuteTestAttack();
            }
        }

        private void ExecuteTestAttack()
        {
            if (playerController == null)
            {
                Debug.LogError("[CombatTester] No refrence to PlayerController!");
                return;
            }

            if (playerController.IsDead)
            {
                Debug.LogWarning("[CombatTester] Player is dead. Test has ended.");
                return;
            }

            float raptorDamage = 25f;
            Debug.LogWarning($"\n[CombatTester] !!! Raptor inficts {raptorDamage} damage points!!!");

            playerController.ApplyDamage(raptorDamage);
        }
    }
}