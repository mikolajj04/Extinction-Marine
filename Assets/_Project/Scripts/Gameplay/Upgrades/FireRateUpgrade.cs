using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay.Controllers;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class FireRateUpgrade : IUpgrade
    {
        public string Title => "[ OVERCLOCK WEAPON ]";
        public string Description => "Increases fire rate by 15%.";
        public int CurrentLevel { get; set; } = 0;
        public int MaxLevel => 100;
        public void Apply(PlayerController player)
        {

            player.ApplyFireRateUpgrade(0.15f);

           
        }
    }
}
