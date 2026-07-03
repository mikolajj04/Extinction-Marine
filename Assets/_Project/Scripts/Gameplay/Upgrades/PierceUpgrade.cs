using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;
using ExtinctionMarine.Gameplay.Upgrades;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    internal class PierceUpgrade : IUpgrade
    {
        public string Title => "[ PIERCING BULLETS! ]";
        public string Description => "+1 penetration for bullets";
        public int CurrentLevel { get; set; } = 0;
        public int MaxLevel => 4;

        public void Apply(PlayerController player)
        {
            player.ApplyPenetrationUpgrade();

        }
    }
}

