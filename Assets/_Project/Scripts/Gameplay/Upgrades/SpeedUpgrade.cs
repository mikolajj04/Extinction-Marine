using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;
using ExtinctionMarine.Gameplay.Controllers;
using ExtinctionMarine.Gameplay.Upgrades;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class SpeedUpgrade : IUpgrade
    {
        public string Title => "[ ENHANCE SERVOS ]";
        public string Description => "Increases movement speed by 1.2";
        public int CurrentLevel { get; set; } = 0;
        public int MaxLevel => 999;

        public void Apply(PlayerController player)
        {
            player.ApplySpeedUpgrade(1.2f);
           
        }
    }
}
