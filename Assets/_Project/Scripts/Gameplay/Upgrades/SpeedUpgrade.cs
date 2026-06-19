using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;
using ExtinctionMarine.Gameplay.Upgrades;
using UnityEngine;

namespace Assets._Project.Scripts.Gameplay.Upgrades
{
    public class SpeedUpgrade : IUpgrade
    {
        public string Title => "[ ENHANCE SERVOS ]";
        public string Description => "Increases movement speed by 1.5.";

        public void Apply(PlayerController player)
        {
            player.MoveSpeed += 1.5f;
            Debug.LogWarning($"[PlayerController] Upgrade has been choosen!: Marine speed increased to {player.MoveSpeed}!");
        }
    }
}
