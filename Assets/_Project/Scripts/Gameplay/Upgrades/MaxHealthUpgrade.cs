using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;
using ExtinctionMarine.Gameplay.Upgrades;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Upgrades
{
    internal class MaxHealthUpgrade : IUpgrade
    {
        public string Title => "[ PAIN TOLERANCE! ]";
        public string Description => "Gain +30 max HP";

        public void Apply(PlayerController player)
        {
           
            Debug.LogWarning("[PlayerController] Upgrade has been choosen!: Marine stimmed! +80HP!");

        }
    }
}

