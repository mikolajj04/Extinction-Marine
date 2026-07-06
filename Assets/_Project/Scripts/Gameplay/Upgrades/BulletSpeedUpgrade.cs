using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay.Controllers;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class BulletSpeedUpgrade : IUpgrade
    {
        public string Title => "[ SUPERSONIC BULLETS ]";
        public string Description => "Increases speed of bullets by 5.";
        public int CurrentLevel { get; set; } = 0;
        public int MaxLevel => 5;
        public void Apply(PlayerController player)
        {

            player.ApplyBulletSpeedUpgrade();


        }
    }
}
