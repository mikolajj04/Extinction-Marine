using System.Collections.Generic;
using System.Linq; 
using ExtinctionMarine.Gameplay.Upgrades;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class UpgradeManager : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private PlayerController player;

       
        private List<IUpgrade> upgradePool;

        private void Awake()
        {
           
            upgradePool = new List<IUpgrade>
            {
                new FireRateUpgrade(),
                new SpeedUpgrade(),
                new HealUpgrade()
            };
        }

       
        public List<IUpgrade> GetRandomUpgrades(int count = 3)
        {
           
            return upgradePool.OrderBy(x => Random.value).Take(count).ToList();
        }

        
        public void SelectUpgrade(IUpgrade chosenUpgrade)
        {
            Debug.Log($"[UpgradeManager] Gracz wybrał: {chosenUpgrade.Title}");

            
            chosenUpgrade.Apply(player);
        }
    }
}