using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;
using ExtinctionMarine.Gameplay.Controllers;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public interface IUpgrade
    {
        string Title { get; }
        string Description { get; }
        int CurrentLevel { get; set; }
        int MaxLevel { get; } 
        void Apply(PlayerController player);
    }
}
