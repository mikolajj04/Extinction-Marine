using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;
using ExtinctionMarine.Gameplay.Upgrades;
using UnityEngine;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class SpeedUpgrade : IUpgrade
    {
        public string Title => "[ ENHANCE SERVOS ]";
        public string Description => "Increases movement speed by 1.2";

        public void Apply(PlayerController player)
        {
            player.MoveSpeed += 1.2f;
            Debug.LogWarning($"[PlayerController] Upgrade has been choosen!: Marine speed increased to {player.MoveSpeed}!");
        }
    }
}
