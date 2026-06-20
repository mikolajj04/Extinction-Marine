using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;
using ExtinctionMarine.Gameplay.UI;
using ExtinctionMarine.Gameplay.Upgrades;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class HealUpgrade : IUpgrade
    {
        public string Title => "[ I NEED MORE STIMS!! ]";
        public string Description => "Instantly restores 80 HP";

        public void Apply(PlayerController player)
        {
            player.ApplyHeal(80f);
            
        }
    }
}
