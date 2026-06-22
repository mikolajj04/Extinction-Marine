using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class FireRateUpgrade : IUpgrade
    {
        public string Title => "[ OVERCLOCK WEAPON ]";
        public string Description => "Increases fire rate by 20%.";
        public void Apply(PlayerController player)
        {

            player.ApplyFireRateUpgrade(0.2f);

           
        }
    }
}
