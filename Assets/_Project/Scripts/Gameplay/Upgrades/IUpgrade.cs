using System;
using System.Collections.Generic;
using System.Text;
using ExtinctionMarine.Gameplay;

namespace ExtinctionMarine.Gameplay.Upgrades
{
    public interface IUpgrade
    {
          string Title { get; }
          string Description { get; }
        void Apply(PlayerController player);
    }
}
