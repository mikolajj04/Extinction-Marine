using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class DamageUpgrade : IUpgrade
    {
        public string Title => "[ Doom-Doom bullets ]";
        public string Description => "Increases damage by 2";
        public int CurrentLevel { get; set; } = 1;
        public int MaxLevel => 100;
        public void Apply(PlayerController player)
        {

            player.ApplyDamageUpgrade();


        }
    }
}
