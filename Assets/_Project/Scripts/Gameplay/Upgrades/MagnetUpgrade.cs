using System;
using System.Collections.Generic;
using System.Text;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public class MagnetUpgrade : IUpgrade 
    {
        public string Title => "[ NANO-MAGNET ]";
        public string Description => "Increases EXP pickup radius by 1.";

        public int CurrentLevel { get; set; } = 1;
        public int MaxLevel => 8;

        public void Apply(PlayerController player)
        {
            player.ApplyMagnetUpgrade(1f);
        }
    }
}
