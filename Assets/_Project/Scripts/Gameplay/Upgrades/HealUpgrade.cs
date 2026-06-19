using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;
using ExtinctionMarine.Gameplay.Upgrades;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Upgrades
{
    public class HealUpgrade : IUpgrade
    {
        public string Title => "[ I NEED MORE STIMS!! ]";
        public string Description => "Instantly restores 80 HP";

        public void Apply(PlayerController player)
        {
            player.LogicData.Heal(80f);
            Debug.LogWarning("[PlayerController] Upgrade has been choosen!: Marine stimmed! +80HP!");
            
        }
    }
}
