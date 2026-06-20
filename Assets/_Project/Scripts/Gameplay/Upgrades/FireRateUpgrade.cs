using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class FireRateUpgrade : IUpgrade
    {
        public string Title => "[ OVERCLOCK WEAPON ]";
        public string Description => "Increases fire rate by 10%.";
        public void Apply(PlayerController player)
        {

            player.FireRate *= 0.90f;

            Debug.LogWarning($"[PlayerController] Upgrade has been choosen!: Fire rate increased to {player.FireRate}!");
        }
    }
}
