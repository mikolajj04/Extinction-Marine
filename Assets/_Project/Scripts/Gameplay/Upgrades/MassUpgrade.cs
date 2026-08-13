using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay.Controllers;
using ExtinctionMarine.Gameplay.Upgrades;

namespace ExtinctionMarine.Gameplay.Upgrades
{
        public class MassUpgrade : IUpgrade
        {
            public string Title => "[ TITANIUM EXOSKELETON ]";
            public string Description => "Increases Marine's mass by 15. High knockback resistance and heavy pushing power.";

            public int CurrentLevel { get; set; } = 0;
            public int MaxLevel => 5;

            public void Apply(PlayerController player)
            {
                
                player.ApplyMassUpgrade(15f);
            }
        }
}

