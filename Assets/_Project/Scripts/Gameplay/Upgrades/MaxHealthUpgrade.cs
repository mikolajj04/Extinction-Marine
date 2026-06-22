using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;
using ExtinctionMarine.Gameplay.Upgrades;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    internal class MaxHealthUpgrade : IUpgrade
    {
        public string Title => "[ PAIN TOLERANCE! ]";
        public string Description => "Gain and heal +20 max HP";

        public void Apply(PlayerController player)
        {
            player.ApplyMaxHealthIncrease(20);

        }
    }
}

