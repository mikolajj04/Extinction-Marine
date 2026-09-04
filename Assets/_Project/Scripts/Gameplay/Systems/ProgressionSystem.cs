using UnityEngine;
using ExtinctionMarine.Gameplay.Controllers;
using System.Runtime.CompilerServices;
using System;
namespace ExtinctionMarine.Gameplay.Systems { 
public class ProgressiuonSystem : MonoBehaviour
{
      
        private void OnEnable()
        {
            EnemyController.OnEnemyKilled += HandleEnemyKilled;
        }
        private void OnDisable()
        {
            EnemyController.OnEnemyKilled -= HandleEnemyKilled;
        }

        private void HandleEnemyKilled(Vector3 vector, float xp, DinosaurSpecies species)
        {
           if(species == DinosaurSpecies.Carnotaurus)
            {
                ArmorySaveData data = SaveSystem.Load<ArmorySaveData>("marine_armory.json");
                data.CarnotaurusKills++;
                if(data.CarnotaurusKills >=1 && !data.IsDashUnlocked)
                {
                    data.IsDashUnlocked = true;
                    Debug.Log($"[Progression] You killed {data.CarnotaurusKills} Carnotaur, Dash Ability Unlocked! ");
                }
                SaveSystem.Save(data, "marine_armory.json");
            } 
        }

      

    }
}