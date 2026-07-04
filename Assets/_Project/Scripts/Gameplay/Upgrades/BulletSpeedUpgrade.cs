using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class BulletSpeedUpgrade : IUpgrade
    {
        public string Title => "[ SUPERSONIC BULLETS ]";
        public string Description => "Increases speed of bullets by X.";
        public int CurrentLevel { get; set; } = 1;
        public int MaxLevel => 100;
        public void Apply(PlayerController player)
        {

            player.ApplyBulletSpeedUpgrade(0.2f);


        }
    }
}
