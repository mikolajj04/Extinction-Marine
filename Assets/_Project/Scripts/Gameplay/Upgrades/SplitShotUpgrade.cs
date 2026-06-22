using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;
using ExtinctionMarine.Gameplay.Upgrades;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class SplitShotUpgrade : IUpgrade
    {
        public string Title => "[ MORE GUNS?! ]";
        public string Description => "Adds an extra projectile to your attack pattern.";


        public int CurrentLevel { get; set; } = 0;


        public int MaxLevel => 3;

        public void Apply(PlayerController player)
        {
            player.ApplySplitShotUpgrade();
        }
    }
}