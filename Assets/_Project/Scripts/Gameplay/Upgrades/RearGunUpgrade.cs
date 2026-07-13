using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;
using ExtinctionMarine.Gameplay.Controllers;
using ExtinctionMarine.Gameplay.Upgrades;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class RearGunUpgrade : IUpgrade
    {
        public string Title => "[ 360° PROTECTION?! ]";
        public string Description => "Adds an extra rear-projectile to your attack pattern.";


        public int CurrentLevel { get; set; } = 0;


        public int MaxLevel => 3;

        public void Apply(PlayerController player)
        {
            player.ApplyRearGunUpgrade();
        }
    }
}