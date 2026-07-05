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
                //new FireRateUpgrade(),
                //new SpeedUpgrade(),
                //new HealUpgrade(),
                //new MaxHealthUpgrade(),
                //new MagnetUpgrade(),
                //new SplitShotUpgrade(),
                //new DamageUpgrade(),
                //new PierceUpgrade(),
                new BulletSpeedUpgrade()

            };
        }

       
        public List<IUpgrade> GetRandomUpgrades(int count = 3)
        {
            var validPool = upgradePool.Where(u => u.CurrentLevel < u.MaxLevel).ToList();


            return validPool.OrderBy(x => Random.value).Take(count).ToList();
        }

        
        public void SelectUpgrade(IUpgrade chosenUpgrade)
        {
            Debug.Log($"[UpgradeManager] Player has choosen: {chosenUpgrade.Title} (previous level of upgrade: {chosenUpgrade.CurrentLevel})");
           

            
            chosenUpgrade.Apply(player);
            chosenUpgrade.CurrentLevel++;
        }
    }
}